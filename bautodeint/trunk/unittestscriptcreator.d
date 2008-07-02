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


module unittestscriptcreator;
private 
{
	import std.stdio;
	import std.file;
	import std.stream;
	import std.string;
}

private string create_progressive_scriptlet()
{
	return `global prog = blankclip.crop(0,0,100,20).loop().scriptclip("""subtitle(string(current_frame))""",after_frame=true).bilinearresize(640,480).converttoyv12().assumefps(25)`;
}

private string create_film_modification()
{
	return `film = prog.assumeframebased().separatefields().selectevery(4,0,1,2,2,3).weave().assumefps(25)`;
}

private string create_interlace_modification()
{
	return `interlace = prog.separatefields().selectevery(4,0,3).weave().assumefps(25)`;
}

private string create_decimation_modification()
{
	return `decimation = prog.selectevery(2, 0, 0, 0, 1, 1).assumefps(25)`;
}

string create_sample(uint num_progressive, uint num_interlace, uint num_film, uint num_decimation)
{
	string filename = "testing.avs";
	BufferedFile temp_file = new BufferedFile(filename, FileMode.OutNew);
	temp_file.writefln(create_progressive_scriptlet());
	temp_file.writefln(create_film_modification());
	temp_file.writefln(create_interlace_modification());
	temp_file.writefln(create_decimation_modification());
	writetrim(temp_file, "prog", num_progressive);
	writetrim(temp_file, "film", num_film);
	writetrim(temp_file, "interlace", num_interlace);
	writetrim(temp_file, "decimation", num_decimation);
	finishOffTrims(temp_file);
	temp_file.writefln("");
	temp_file.close();
	return filename;
}

private void writetrim(BufferedFile temp_file, string stream_name, uint num_frames)
{
	if (num_frames == 0)
		return;
	temp_file.writef("%s.trim(0, -%d) ++ ", stream_name, num_frames);
}

private void finishOffTrims(BufferedFile temp_file)
{
	temp_file.writef("prog.trim(0,-1)");
}