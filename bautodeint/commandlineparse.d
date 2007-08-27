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

module commandlineparse;
private import std.stdio;
private import std.conv;

version (Windows) const int LINE_LENGTH = 80;

enum ArgumentType { NONE, INTEGER, FLOAT, STRING };

class CommandlineException : Exception
{
	this(string message) 
	{
		super(message);
	}
}


template clipToRange(TDataType)
{
	TDataType clipToRange(TDataType number, TDataType min, TDataType max) 
	{
		if (number < min)
			return min;
		if (number > max)
			return max;
		return number;
	}
}

class CommandlineParser
{
	unittest
	{
		writefln("Starting CommandlineParser unit tests");
		string[] args = new string[2];
		args[0] = "--input";
		args[1] = "filename.txt";
		CommandlineParser p = new CommandlineParser(args);
		assert (p.hasStringArgument("--input"));
		assert (p.getStringArgument("--input") == "filename.txt");
		try
		{
			assert (!p.hasIntArgument("--input"));
			assert (0);
		}
		catch (Error)
		{
			assert (1);
		}
		try 
		{
			assert (!p.hasFloatArgument("--input"));
			assert (0);
		}
		catch (Error)
		{
			assert (1);
		}
		try
		{
			assert (!p.hasStringArgument("--rubbish"));
			assert (0);
		}
		catch (Error)
		{
			assert (1);
		}

		try
		{
			// --garbage is not a supported argument, so this should throw an error
			args.length = 1;
			args[0] = "--garbage";
			p = new CommandlineParser(args);
			assert (false);
		}
		catch (CommandlineException)
		{
			assert (true);
		}

		try 
		{
			// --input is missing a following argument, so this should throw an error
			args[0] = "--input";
			p = new CommandlineParser(args);
			assert (false);
		}
		catch (CommandlineException)
		{
			assert (true);
		}
		writefln("Testing printHelp() function. Please check whether formatting is pretty.");
		register("--dud-argument", ArgumentType.STRING, "this is a very very very very very very very very very very very very very very very very very very very very very very very very very long string whose attempt is to wrap multiple times to see whether the wrapping works");
		printHelp();
		unregister("--dud-argument");
		writefln("CommandlineParser unit tests completed successfully!");
	}



	private
	{
		static ArgumentType[string] supportedArguments;
		static string[string] descriptions;
		string[string] stringArgs;
		int[string] intArgs;
		bool[string] boolArgs;
		double[string] floatArgs;
	}
	
	static this()
	{
		register("--input", ArgumentType.STRING, "input *.avs file to analyse");
		register("--min-useful-sections", ArgumentType.INTEGER, "minimum useful sections required for confidence");
		register("--min-analysis-sections", ArgumentType.INTEGER, "minimum sections to analyse");
		register("--hybrid-type-threshold", ArgumentType.FLOAT, "threshold for type to be declared hybrid");
		register("--hybrid-f-o-threshold", ArgumentType.FLOAT, "threshold for field order to be declared hybrid");
		register("--analysis-percent", ArgumentType.FLOAT, "percent of video to analyse");
		register("--message-level", ArgumentType.INTEGER, "level of messages to receive: 0: errors (minimum); 1: warnings; 2: info; 3: debug");
		register("--min-combed-frames", ArgumentType.FLOAT, "unused at the moment");
		register("--decimation-threshold", ArgumentType.FLOAT, "unused at the moment");
		register("--portion-threshold", ArgumentType.FLOAT, "unused at the moment");
		register("--max-portions", ArgumentType.INTEGER, "unused at the moment");
		register("--portions", ArgumentType.NONE, "unused at the moment");
		register("--anime", ArgumentType.NONE, "source is anime (unused at the moment)");
	}

	static
	{
		void printHelp()
		{
			string toString(ArgumentType type)
			{
				switch (type)
				{
					case ArgumentType.NONE:
						return "";
					case ArgumentType.FLOAT:
						return "<float>";
					case ArgumentType.INTEGER:
						return "<int>";
					case ArgumentType.STRING:
						return "<string>";
				}
			}
			void printarg(string arg, string argumenttype, string description)
			{
				writefln("%-23s %-8s   %s", arg, argumenttype, description);
			}
			void printshortarg(string description)
			{
				printarg("", "", description);
			}
			
			writefln("\nUsage:");
			const int LEFT_HAND_LENGTH = 35; // This is the length of the parameter and the type it takes
			const int RIGHT_HAND_LENGTH = LINE_LENGTH - LEFT_HAND_LENGTH - 1;
			foreach (string key ; supportedArguments.keys)
			{
				if (descriptions[key].length <= RIGHT_HAND_LENGTH) // In this case it only spans one line, so print it normally
				{
					printarg(key, toString(supportedArguments[key]), descriptions[key]);
				}
				else // Do a special treatment so that the formatting still looks nice
				{
					string description = descriptions[key];
					printarg(key, toString(supportedArguments[key]), description[0..RIGHT_HAND_LENGTH]);
					int location = RIGHT_HAND_LENGTH;
					while ((location + RIGHT_HAND_LENGTH) < description.length)
					{
						printshortarg(descriptions[key][location..(location+RIGHT_HAND_LENGTH)]);
						location += RIGHT_HAND_LENGTH;
					}
					printshortarg(description[location..length]);
				}
			}
		}
		static void register(string tag, ArgumentType type, string description)
		{
			supportedArguments[tag] = type;
			descriptions[tag] = description;
		}

		static void unregister(string tag)
		in
		{
			assert ((tag in supportedArguments) !is null);
			assert ((tag in descriptions) !is null);
		}
		body
		{
			supportedArguments.remove(tag);
			descriptions.remove(tag);
		}
	}

	invariant()
	{
		assert (descriptions.keys.length == supportedArguments.length);
		foreach (string key ; supportedArguments.keys)
		{
			assert ((key in descriptions) !is null);
		}
	}

	private bool isSupported(string argument, ArgumentType type)
	{
		if ((argument in supportedArguments) is null)
			return false;
		return (supportedArguments[argument] == type);
	}
		
	public 
	{
		bool hasBoolArgument(string argument)
		in 
		{
			assert (isSupported(argument, ArgumentType.NONE));
		}
		body
		{
			return (argument in boolArgs) != null;
		}

		bool hasIntArgument(string argument)
		in 
		{
			assert (isSupported(argument, ArgumentType.INTEGER));
		}
		body
		{
			return (argument in intArgs) != null;
		}
		
		int getIntArgument(string argument)
		in
		{
			assert (hasIntArgument(argument));
		}
		body
		{
			return intArgs[argument];
		}

		bool hasFloatArgument(string argument)
		in 
		{
			assert (isSupported(argument, ArgumentType.FLOAT));
		}
		body
		{
			return (argument in floatArgs) != null;
		}

		double getFloatArgument(string argument)
		in
		{
			assert (hasFloatArgument(argument));
		}
		body
		{
			return floatArgs[argument];
		}

		bool hasStringArgument(string argument)
		in 
		{
			assert (isSupported(argument, ArgumentType.STRING));
		}
		body
		{
			return ((argument in stringArgs) != null);
		}

		string getStringArgument(string argument)
		in
		{
			assert (hasStringArgument(argument));
		}
		body
		{
			return stringArgs[argument];
		}

		this(string[] args)
		{
			bool usedArgument = false;
			foreach (int i, string arg; args)
			{
				if (usedArgument)
				{ 
					usedArgument = false;
					continue;
				}
				if ((arg in supportedArguments) == null)
				{
					throw new CommandlineException("Unsupported argument: '" ~ arg ~ "'");
				}
				ArgumentType type = supportedArguments[arg];

				if (type == ArgumentType.NONE)
					boolArgs[arg] = true;
				else
				{
					if (i >= args.length - 1)
						throw new CommandlineException("Expected argument after '" ~ arg ~ "'");
					try 
					{
						switch (type)
						{
							case ArgumentType.STRING:
								stringArgs[arg] = args[i+1];
								break;

							case ArgumentType.INTEGER:
								intArgs[arg] = toInt(args[i+1]);
								break;

							case ArgumentType.FLOAT:
								floatArgs[arg] = std.conv.toDouble(args[i+1]);
								break;
						}
					}
					catch (std.conv.ConvError)
					{
						throw new CommandlineException("Argument '" ~ args[i+1] ~ "' was of the incorrect format");
					}
					catch (std.conv.ConvOverflowError)
					{
						throw new CommandlineException("Argument '" ~ args[i+1] ~ "' was too large");
					}
					usedArgument = true;
				}
			}
		}
	}
}
