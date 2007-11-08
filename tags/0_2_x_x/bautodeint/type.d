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

module type;

enum SourceType { UNKNOWN, PROGRESSIVE, INTERLACED, FILM, 
					DECIMATING, HYBRID_FILM, PARTLY_INTERLACED };

enum SectionType { UNKNOWN, PROGRESSIVE, INTERLACED, FILM };

enum MessageImportance { ERROR, WARNING, INFO, VERBOSE, DEBUG};

/* This way, we can just convert this as an int
 * to a string and we'll get a good avisynth field 
 * order: -1 for AviSynth internal value, 0 for bff 
 * and 1 for tff */
enum FieldOrder
{
	UNKNOWN = -2,
	VARIABLE = -1,
	BFF = 0,
	TFF = 1
}

dstring toString(MessageImportance type)
{
	switch (type)
	{
		case MessageImportance.ERROR:
			return "error";
		case MessageImportance.WARNING:
			return "warning";
		case MessageImportance.INFO:
			return "info";
		case MessageImportance.VERBOSE:
			return "verbose";
		case MessageImportance.DEBUG:
			return "debug";
	}
}

dstring toString(SourceType type)
{
	switch (type)
	{
		case SourceType.UNKNOWN:
			return "unknown";
		case SourceType.PROGRESSIVE:
			return "progressive";
		case SourceType.INTERLACED:
			return "interlaced";
		case SourceType.FILM:
			return "film";
		case SourceType.DECIMATING:
			return "decimating";
		case SourceType.HYBRID_FILM:
			return "partly film";
		case SourceType.PARTLY_INTERLACED:
			return "partly interlaced";
	}
}

dstring st_toString(SectionType type)
{
	switch (type)
	{
		case SectionType.UNKNOWN:
			return "unknown";
		case SectionType.PROGRESSIVE:
			return "progressive";
		case SectionType.FILM:
			return "film";
		case SectionType.INTERLACED:
			return "interlaced";
	}
}

dstring fo_toString(FieldOrder type)
{
	switch (type)
	{
		case FieldOrder.UNKNOWN:
			return "unknown";
		case FieldOrder.VARIABLE:
			return "variable";
		case FieldOrder.BFF:
			return "bff";
		case FieldOrder.TFF:
			return "tff";
	}
}