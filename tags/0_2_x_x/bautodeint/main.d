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


private
{
	import commandlineparse;
	import clip;
	import type;
	import std.stdio;
	import std.c.stdio;
}

int main(string[] args)
{
	CommandlineParser p = new CommandlineParser(args[1..length]);
	MessageImportance m_level = MessageImportance.INFO;
	debug m_level = MessageImportance.DEBUG;
	if (p.hasIntArgument("--message-level"))
		m_level = cast(MessageImportance) (clipToRange!(int)(p.getIntArgument("--message-level"), 0, 3));
	bool hasError = false;
	void message(MessageImportance level, dstring msg)
	{
		writefln("BAutoDeint [%s]: %s", toString(level), msg);
		if (level == MessageImportance.ERROR) hasError = true;
	}
	uint last_status = 0;
	void status(uint number, uint total)
	{
		if ((100*number / total) != last_status)
		{
			last_status = 100*number / total;
			writef("processed frames: %d/%d -- %d%% completed    \r", number, total, last_status);
			fflush(stdout);
		}
	}
	if (p.hasStringArgument("--input"))
	{
		SourceDetectorSettings settings = createSettings(p);
		Clip c = new Clip(&message, &status, settings, p.getStringArgument("--input"), m_level, p.hasBoolArgument("--anime"));
		c.process();
		if (!hasError)
		{
			writefln("\nProcessing completed. Type is determined to be %s.", toString(c.info.type));
			if (c.info.needs_FO)
				writefln("Field order is %s.", fo_toString(c.info.field_order));
		}
	}
	else
	{
		CommandlineParser.printHelp();
	}
	return 0;
}

SourceDetectorSettings createSettings(CommandlineParser p)
{
	SourceDetectorSettings value;
	if (p.hasIntArgument("--min-useful-sections"))
		value.minimum_useful_sections = p.getIntArgument("--min-useful-sections");
	if (p.hasIntArgument("--min-analysis-sections"))
		value.minimum_analyse_sections = p.getIntArgument("--min-analysis-sections");
	if (p.hasFloatArgument("--hybrid-type-threshold"))
		value.hybrid_type_percent = p.getFloatArgument("--hybrid-type-threshold");
	if (p.hasFloatArgument("--hybrid-f-o-threshold"))
		value.hybrid_field_order_percent = p.getFloatArgument("--hybrid-f-o-threshold");
	if (p.hasFloatArgument("--min-combed-frames"))
		value.combed_frame_minimum = p.getFloatArgument("--min-combed-frames");
	if (p.hasFloatArgument("--decimation-threshold"))
		value.decimation_threshold = p.getFloatArgument("--decimation-threshold");
	if (p.hasFloatArgument("--portion-threshold"))
		value.portion_threshold = p.getFloatArgument("--portion-threshold");
	if (p.hasIntArgument("--max-portions"))
		value.max_portions = p.getIntArgument("--max-portions");
	if (p.hasBoolArgument("--portions"))
		value.portions_allowed = true;
	return value;
}
