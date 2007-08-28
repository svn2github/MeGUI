// ****************************************************************************
// 
// Copyright (C) 2007 berrinam
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

//////////// Written in the D programming language

module clip;
private 
{
	import unittestscriptcreator;
	import type;
	import avireader;
	import scriptserver;
	import std.math;
	import std.file;
	import std.stdio;
	import std.stream;
	import std.string;
	import std.format;
	import std.conv;
}
const uint SECTION_LENGTH = 15;
const uint FIELD_SECTION_LENGTH = 5;

class InternalAutoDeintException : Exception 
{
	public this(string msg) 
	{
		super(msg);
	}
}

class Clip
{
	unittest
	{
		bool hasError = false;
		writefln("Starting module clip's unit tests");
		// Testing get_modal_type
		writef("Unit-testing get_modal_type...");
		uint[SectionType] types;
		int frequency;
		types[SectionType.UNKNOWN] = 1;
		assert (get_modal_type(types, frequency) == SectionType.UNKNOWN);
		assert (frequency == 1);
		types[SectionType.FILM] = 2;
		assert (get_modal_type(types, frequency) == SectionType.FILM);
		assert (frequency == 2);
		types.remove(SectionType.FILM);
		types.remove(SectionType.UNKNOWN);
		get_modal_type(types, frequency);
		assert (frequency == -1);
		writefln("Succeeded!");

		// Testing get_short_section_type
		writef("Unit-testing get_short_section_type...");
		bool[] comb_data = new bool[5];
		comb_data[] = false;
		bool[] motion_data = new bool[5];
		motion_data[] = true;
		assert (get_short_section_type(comb_data, motion_data) == SectionType.PROGRESSIVE);
		comb_data[0] = true;
		assert (get_short_section_type(comb_data, motion_data) == SectionType.UNKNOWN);
		comb_data[1] = true;
		assert (get_short_section_type(comb_data, motion_data) == SectionType.FILM);
		comb_data[1] = false;
		comb_data[4] = true;
		assert (get_short_section_type(comb_data, motion_data) == SectionType.FILM);
		comb_data[4] = false;
		comb_data[3] = true;
		assert (get_short_section_type(comb_data, motion_data) == SectionType.INTERLACED);
		comb_data[] = true;
		assert (get_short_section_type(comb_data, motion_data) == SectionType.INTERLACED);
		writefln("Succeeded!");

		// Testing get_section_type
		try
		{
			
			writef("Unit-testing get_section_type...");
			comb_data.length = 15;
			motion_data.length = 15;
			comb_data[] = true;
			motion_data[] = true;
			assert (get_section_type(comb_data, motion_data) == SectionType.INTERLACED);
			comb_data[] = false;
			comb_data[0] = true;
			comb_data[4..6] = true;
			comb_data[9..11] = true;
			assert (get_section_type(comb_data, motion_data) == SectionType.FILM);
			motion_data[14] = false;
			assert (get_section_type(comb_data, motion_data) == SectionType.FILM);
			motion_data[] = true;
			motion_data[0] = false;
			assert (get_section_type(comb_data, motion_data) == SectionType.UNKNOWN);
			motion_data[] = true;
			comb_data[] = false;
			assert (get_section_type(comb_data, motion_data) == SectionType.PROGRESSIVE);
			comb_data[] = false;
			for (int i = 0; i < comb_data.length; i++)
			{
				comb_data[i] = true;
				assert (get_section_type(comb_data, motion_data) == SectionType.PROGRESSIVE);
				comb_data[i] = false;
			}
			writefln("Succeeded!");
		}
		catch (Exception ae)
		{
			writefln(ae.msg);
			foreach (bool b; comb_data)
				writef(cast(int)b);
			writefln("(comb_data)");
			foreach (bool b; motion_data)
				writef(cast(int)b);
			writefln("(motion_data)");
			hasError = true;
		}

		version (FULL_TEST)
		{
			writefln("Unit-testing Clip class with various scripts...");
			SourceDetectorSettings sds;
			void test_script(uint num_progressive, uint num_interlaced, uint num_film, 
			uint num_decimation, SourceType expectedResult, string sourceTypeName)
			{
				writef("\tTesting %s clip of %d frames...", sourceTypeName, (num_progressive + num_interlaced + num_film + num_decimation));
				fflush(stdout);
				string temp_scriptname = create_sample(num_progressive, num_interlaced, num_film, num_decimation);
				Clip c = new Clip(null, null, sds, temp_scriptname, MessageImportance.DEBUG, false);
				c.process();
				try
				{
					assert (c.info.type == expectedResult);
					writefln("succeeded!");
				}
				catch (Exception)
				{
					writefln("Failed:\n\t\tExpected type: %s Type found: %s", type.toString(expectedResult), type.toString(c.info.type));
					hasError = true;
				}
			}
			test_script(2000, 0, 0, 0, SourceType.PROGRESSIVE, "pure progressive");
			test_script(0, 2000, 0, 0, SourceType.INTERLACED, "pure interlaced");
			test_script(0, 0, 2000, 0, SourceType.FILM, "pure film");
			test_script(0, 0, 0, 2000, SourceType.DECIMATING, "pure decimation");
			test_script(2000, 2000, 0, 0, SourceType.PARTLY_INTERLACED, "partly interlaced");
			test_script(0, 2000, 2000, 0, SourceType.HYBRID_FILM, "hybrid film");
			test_script(1000, 1000, 1000, 0, SourceType.HYBRID_FILM, "hybrid film/ntsc/progressive");
			test_script(2000, 0, 2000, 0, SourceType.HYBRID_FILM, "hybrid film/progressive");
		}
		if (hasError)
			writefln("module clip had some errors in unit tests");
		else
			writefln("module clip completed unit tests successfully");
	}

	this(void delegate(MessageImportance, dstring message) message, void delegate(uint number, uint total) status,
			SourceDetectorSettings settings, string filename, MessageImportance level, bool isAnime)
	in 
	{
		assert (filename != null);
	}
	body
	{
		m_level = level;
		m_settings = settings;
		m_filename = filename;
		m_message = message;
		m_status = status;
		m_clip_info.is_anime = isAnime;
	}

	public ClipInfo info()
	{
		return m_clip_info;
	}

	public void process()
	in
	{
		assert(m_filename != null);
	}
	body
	{
		uint[SectionType] sectionCounts;
		manageEntirePass!(SectionType, bool)(ScriptType.DETECTION, m_clip_info.all_sections,
											m_comb_data, m_motion_data, SECTION_LENGTH, &get_section_type, &(type.st_toString), sectionCounts);
		messagef(MessageImportance.DEBUG, "Processing section data...");
		process_counts(sectionCounts);
		check_for_decimation();
		if (m_clip_info.needs_FO)
		{
			uint[FieldOrder] fieldCounts;
			manageEntirePass!(FieldOrder, double)(ScriptType.FIELD_ORDER, m_clip_info.all_field_orders,
												m_field_data[0], m_field_data[1], FIELD_SECTION_LENGTH,
												&get_field_type, &(type.fo_toString), fieldCounts);
			messagef(MessageImportance.DEBUG, "Processing field order data...");
			process_counts(fieldCounts);
		}
	}


	private 
	{
		void check_for_decimation()
		{
			if (m_clip_info.type != SourceType.UNKNOWN && m_clip_info.type != SourceType.PROGRESSIVE)
				return;
			uint numMovingFrames(bool[] motion)
			in
			{
				assert (motion.length == 5);
			}
			out (value)
			{
				assert (0 <= value && value <=5);
			}	
			body
			{
				uint motionAmount;
				foreach (bool m ; motion)
					if (m) motionAmount++;
				return motionAmount;
			}

			bool modalMovingFrames(uint[] numbers, out uint numMoving)
			{
				uint maxSoFar = 0;
				uint maxKey = 0;
				foreach (int index, uint value; numbers)
					if (value > maxSoFar)
					{
						maxSoFar = value;
						maxKey = index;
					}
				numMoving = maxKey;
				if (maxSoFar >= 2 && numMoving != 0)
					return true;
				return false;
			}
			uint max(uint[] numbers, int excluding, out int i)
			{
				uint max = 0;
				foreach (int index, uint number ; numbers)
					if (number > max && index != excluding)
					{
						max = number;
						i = index;
					}
				return max;
			}
			
			uint[] numSectionsWithMotionPattern = new uint[6];
			uint[] localMotionPattern = new uint[6];
			uint count = 0;
			for (uint index = 0; index < m_motion_data.length; index += 5)
			{
				localMotionPattern[numMovingFrames(m_motion_data[index..index+5])]++;
				count++;
				if (count == 3)
				{
					count = 0;
					uint numMoving;
					if (modalMovingFrames(localMotionPattern, numMoving))
						numSectionsWithMotionPattern[numMoving]++;
					localMotionPattern[] = 0;
				}
			}

			foreach (int index, uint value ; numSectionsWithMotionPattern)
				messagef(MessageImportance.INFO, "There are %d sections with %d frames moving.", value, index);

			int maxIndex, ignored;
			uint maxValue = max(numSectionsWithMotionPattern, -1, maxIndex);
			uint secondMax = max(numSectionsWithMotionPattern, maxIndex, ignored);
			if (maxValue > cast(double)secondMax * m_settings.decimation_threshold && maxIndex != 5 && maxIndex != 0)
			{
				m_clip_info.type = SourceType.DECIMATING;
				m_clip_info.decimation = maxIndex;
			}
		}

		template manageEntirePass(TSectionDataType, TDataType)
		{
			void manageEntirePass(ScriptType s_type, out TSectionDataType[] results, 
				out TDataType[] dataA, out TDataType[] dataB, uint sectionLength, 
				TSectionDataType function(TDataType[] dataA, TDataType[] dataB) getResult,
				dstring function(TSectionDataType) toString, out uint[TSectionDataType] sectionCounts)
			{
				uint numSections = dataReading!(TDataType).getResults(s_type, dataA, dataB, sectionLength);
				uint newLength = sectionLength * numSections;
				results = new TSectionDataType[numSections];
			
				for (int iData = 0, iSection = 0; iData < newLength; iData += sectionLength, iSection++)
				{
					TSectionDataType type = getResult(dataA[iData..iData+sectionLength],
														dataB[iData..iData+sectionLength]);
					results[iSection] = type;
					if (!(type in sectionCounts))
						sectionCounts[type] = 0;
					sectionCounts[type]++;
				}
				foreach (TSectionDataType t, uint count; sectionCounts)
				{
					messagef(MessageImportance.INFO, "Number of sections of type `%s': %d", toString(t), count);
				}
			}
				
		}

		
		private void process_counts(uint[SectionType] sectionCounts)
		{
			uint count(SectionType type)
			{
				if (type in sectionCounts)
					return sectionCounts[type];
				else
					return 0;
			}
			uint sectioncount = m_clip_info.all_sections.length;
			uint num_known_sections = sectioncount - count(SectionType.UNKNOWN);
			if (num_known_sections < m_settings.minimum_useful_sections)
			{
				m_clip_info.type = SourceType.UNKNOWN;
				return;
			}
			bool has(SectionType type)
			{
				return (count(type) >= (m_settings.hybrid_type_percent/100.0)*cast(double)sectioncount);
			}
			bool has_film = has(SectionType.FILM);
			bool has_interlace = has(SectionType.INTERLACED);
			bool has_progressive = has(SectionType.PROGRESSIVE);
			m_clip_info.needs_FO = true;
			if (has_film)
			{
				if (has_interlace || has_progressive)
				{
					m_clip_info.type = SourceType.HYBRID_FILM;
					m_clip_info.mostly_film = (sectionCounts[SectionType.FILM] > (count(SectionType.PROGRESSIVE) + count(SectionType.INTERLACED)));
				}
				else
				{
					m_clip_info.type = SourceType.FILM;
				}
			}
			else if (has_interlace)
			{
				if (has_progressive)
				{
					m_clip_info.type = SourceType.PARTLY_INTERLACED;
				}
				else
				{
					m_clip_info.type = SourceType.INTERLACED;
				}
			}
			else
			{
				m_clip_info.needs_FO = false;
				m_clip_info.type = SourceType.PROGRESSIVE;
			}
		}

		private void process_counts(uint[FieldOrder] orderCounts)
		{
			uint count(FieldOrder type)
			{
				if (type in orderCounts)
					return orderCounts[type];
				else return 0;
			}
			uint sectioncount = m_clip_info.all_field_orders.length;
			messagef(MessageImportance.DEBUG, "Number of field-order sections: %d", sectioncount);
			bool has(FieldOrder type)
			{
				return (count(type) >= (cast(double)sectioncount * m_settings.hybrid_field_order_percent / 100.0));
			}
			bool hasTFF = has(FieldOrder.TFF);
			bool hasBFF = has(FieldOrder.BFF);
			messagef(MessageImportance.DEBUG, "TFF: %s BFF: %s", hasTFF, hasBFF);
			if (hasTFF && !hasBFF)
				m_clip_info.field_order = FieldOrder.TFF;
			else if (hasBFF && !hasTFF)
				m_clip_info.field_order = FieldOrder.BFF;
			else if (hasBFF && hasTFF)
				m_clip_info.field_order = FieldOrder.VARIABLE;
			else m_clip_info.field_order = FieldOrder.UNKNOWN;
		}

		private static SectionType get_modal_type(uint[SectionType] types, out int frequency)
		out (type)
		{
			assert( (!(type in types)) || (types[type] == frequency));
			assert (types.keys.length == 0 || frequency >= 0);
		}
		body
		{
			SectionType mostCommonSectionType;
			frequency = -1;
			foreach (SectionType type ; types.keys)
			{
				if (cast(int)types[type] > frequency)
				{
					mostCommonSectionType = type;
					frequency = types[type];
				}
			}
			return mostCommonSectionType;
		}

		private static FieldOrder get_field_type(double[] dataA, double[] dataB)
		in
		{
			assert (dataA.length == dataB.length);
			assert (dataB.length == FIELD_SECTION_LENGTH);
		}
		body
		{
			int countA = 0, countB = 0;
			// We start at 1, because 0 is a scene-change, which stuffs things up
			for (int iData = 1; iData < dataA.length; iData++)
			{
				if (dataA[iData] > dataB[iData])
					countA++;
				else if (dataB[iData] > dataA[iData])
					countB++;
			}
			if (countA > countB && countB == 0)
				return FieldOrder.TFF;
			else if (countB > countA && countA == 0)
				return FieldOrder.BFF;
			return FieldOrder.UNKNOWN;
		}
			

		private static SectionType get_section_type(bool[] comb_data, bool[] motion_data)
		in
		{
			assert (comb_data.length == SECTION_LENGTH);
			assert (motion_data.length == SECTION_LENGTH);
			assert (SECTION_LENGTH % 5 == 0);
		}
		body
		{
			uint[SectionType] sectionTypes;
			for (int iData = 0; iData < comb_data.length; iData+= 5)
			{
				SectionType type = get_short_section_type(comb_data[iData..iData+5],motion_data[iData..iData+5]);
				if (!(type in sectionTypes))
					sectionTypes[type] = 0;
				sectionTypes[type]++;
			}
			int frequency;
			SectionType type = get_modal_type(sectionTypes, frequency);
			if (frequency >= 2)
				return type;
			return SectionType.UNKNOWN;
		}

		private static SectionType get_short_section_type(bool[] comb_data, bool[] motion_data)
		in
		{
			assert (comb_data.length == 5);
			assert (motion_data.length == 5);
		}
		body
		{
			foreach (bool frame_moving ; motion_data)
				if (!frame_moving)
					return SectionType.UNKNOWN;

			int[] combedFrames = new int[2];
			combedFrames[] = -1;
			int numCombed = 0;
			foreach (int i, bool frame_combed ; comb_data)
			{
				if (frame_combed)
				{
					if (numCombed < combedFrames.length)
						combedFrames[numCombed] = i;
					numCombed++;
				}
			}
			if (numCombed == 0)
				return SectionType.PROGRESSIVE;
			
			if (numCombed == 2 && (combedFrames[1]-combedFrames[0] == 1) || combedFrames[1]-combedFrames[0] == 4)
				return SectionType.FILM;

			if (numCombed > 1)
				return SectionType.INTERLACED;

			return SectionType.UNKNOWN;
		}

		template dataReading(TData)
		{
			private uint getResults(ScriptType type, out TData[] dataA, out TData[] dataB, uint sectionLength)
			{
				string data_name, temp_scriptname;
				temp_scriptname = create_temp_script(type, data_name);
				messagef(MessageImportance.DEBUG, "Created temporary script `%s' with temporary data file `%s'", temp_scriptname, data_name);
				messagef(MessageImportance.DEBUG, "Reading through script...");
				avireader.readThroughAvi(temp_scriptname, m_status);
				if (! exists(data_name) )
				{
					throw new InternalAutoDeintException("File " ~ data_name ~ "' is unexpectedly missing. Cannot resume");
				}
				messagef(MessageImportance.DEBUG, "Reading data from `%s'...", data_name);
				read_data(data_name, dataA, dataB, &(dataParsing!(TData).getData), dataParsing!(TData).expectedLineLength);
				uint newLength = sectionLength * cast(uint)(dataA.length / sectionLength);
				dataA.length = newLength;
				dataB.length = newLength;
				uint numSections = newLength / sectionLength;
				messagef(MessageImportance.DEBUG, "There are %d frames and %d sections", newLength, numSections);
				debug assert (newLength % sectionLength == 0);
				return numSections;
			}
			private static void read_data(string data_file, out TData[] dataA, out TData[] dataB, 
											TData function(string) getData, uint estimatedLineLength)
			in 
			{
				assert (exists(data_file));
			}
			out
			{
				assert (dataA.length == dataB.length);
			}
			body
			{
				ulong filesize = getSize(data_file);
				uint guessedLines = cast(uint)(filesize / estimatedLineLength);
				BufferedFile input = new BufferedFile(data_file, FileMode.In);
				dataA = new TData[guessedLines];
				dataB = new TData[guessedLines];
				int i = 0;
				while (!input.eof())
				{
					string nextLine = input.readLine();
					int separatorIndex = find(nextLine, '-');
					if (separatorIndex == -1) throw new Exception("Error in data-file");
					debug assert (dataA.length == dataB.length);
					if (dataA.length <= i)
					{
						dataA.length = dataA.length + 100;
						dataB.length = dataB.length + 100;
					}
					dataA[i] = getData(nextLine[0..separatorIndex]);
					dataB[i] = getData(nextLine[separatorIndex+1..length]);
					i++;
				}
				input.close();
				dataA.length = i;
				dataB.length = i;
			}		
		}
		
/+		template read_data(TData)
		{

		}+/

		template dataParsing(TData : double)
		{
			private static double getData(string text)
			{
				return toDouble(text);
			}
			static uint expectedLineLength = 20;
		}

		template dataParsing(TData : bool)
		{
			private static bool getData(string text)
			{
				if (text == "true")
					return true;
				else if (text == "false") 
					return false;
				throw new Exception("Error in data-file");
			}
			static uint expectedLineLength = 12;
		}

		private string create_temp_script(ScriptType type, out string data_name)
		in 
		{
			assert (m_filename != null);
		}
		body
		{
			string temp_scriptname = m_filename ~ ".bautodeint_temp.avs";
			data_name = m_filename ~ ".bautodeint_temp.data";
			BufferedFile temp_file = new BufferedFile(temp_scriptname, FileMode.OutNew);
			int selectEvery = getSelectEvery();
			if (type == ScriptType.DETECTION)
			{
				temp_file.writefln(DETECTION_SCRIPT, m_filename, data_name, selectEvery, SECTION_LENGTH);
			}
			else // type == ScriptType.field_order
			{
				data_name ~= ".fieldorder";
				temp_file.writefln(FIELD_ORDER_SCRIPT, m_filename, data_name, selectEvery, FIELD_SECTION_LENGTH);
			}
			temp_file.close();
			if (exists(data_name))
				std.file.remove(data_name);
			return temp_scriptname;
		}


		private int getNumFrames()
		{
			AviFile avi = new AviFile();
			avi.openFile(m_filename);
			int value = avi.framecount;
			avi.close();
			return value;
		}

		private int getSelectEvery()
		{
			const double selectLength = SECTION_LENGTH;
			int numFrames = getNumFrames();
			double realAnalysePercent = fmax(m_settings.analyse_percent, 
					100.0 * (cast(double)m_settings.minimum_analyse_sections) /  (cast(double)getNumFrames/SECTION_LENGTH));
			int selectEvery = cast(int) (100.0 * selectLength / realAnalysePercent);
			selectEvery = cast(int)fmax(selectEvery, 15);
			return selectEvery;
		}

		private void messagef(MessageImportance level, ...)
		{
			if (m_message is null || level > m_level)
				return;
			uint mlength = 0;
			dchar[] message = new dchar[100];
			void putc(dchar c)
			{
				if (message.length <= mlength)
				{
					message.length = message.length + 100;
				}
				message[mlength] = c;
				mlength++;
			}
			doFormat(&putc, _arguments, _argptr);
			message.length = mlength;
			m_message(level, message);
		}
	}
	
	private 
	{
		void delegate(MessageImportance, dstring) m_message;
		void delegate(uint, uint) m_status;
		final MessageImportance m_level;
		final SourceDetectorSettings m_settings;
		final string m_filename;
		bool[] m_comb_data;
		bool[] m_motion_data;
		double[][2] m_field_data;
		bool m_has_error, m_continue_working;
		string m_errorMessage;
		ClipInfo m_clip_info;
	}
}

unittest
{

}

struct Portion
{
	SectionType type;
	int startSection;
	int numSections;
}

struct ClipInfo
{
	bool is_anime;
	SourceType type;
	FieldOrder field_order;
	bool mostly_film;
	bool needs_FO;
	FieldOrder[] all_field_orders;
	SectionType[] all_sections;
	uint decimation;
}

struct SourceDetectorSettings
{
	uint minimum_useful_sections = 30;
	uint minimum_analyse_sections = 150;
	double hybrid_type_percent = 5;
	double hybrid_field_order_percent = 10;
	double analyse_percent = 3;
	double combed_frame_minimum = 5;
	double decimation_threshold = 2.0;
	double portion_threshold = 5.0;
	uint max_portions = 5;
	bool portions_allowed = false;
}