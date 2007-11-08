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

module scriptserver;

enum ScriptType { DETECTION, FIELD_ORDER };

const string DETECTION_SCRIPT = 
`Import("%s") #original script filename
global unused_ = blankclip(pixel_type="yv12", length=10).TFM()
file="%s"
global sep="-"
function IsMoving() {
  global b = (diff < 1.0) ? false : true}
c = last
global clip = last
c = WriteFile(c, file, "a", "sep", "b")
c = FrameEvaluate(c, "global a = IsCombedTIVTC(clip, cthresh=9)")
c = FrameEvaluate(c, "IsMoving")
c = FrameEvaluate(c,"global diff = YDifferenceFromPrevious(clip)")
crop(c,0,0,16,16)
SelectRangeEvery(%d,%d,0)
`;

const string FIELD_ORDER_SCRIPT = 
`Import("%s") # original script filename
file="%s"
global sep="-"
d = last
global abff = d.assumebff().separatefields()
global atff = d.assumetff().separatefields()
c = d.loop(2)
c = WriteFile(c, file, "diffa", "sep", "diffb")
c = FrameEvaluate(c,"global diffa = 0.50*YDifferenceFromPrevious(abff) + 0.25*UDifferenceFromPrevious(abff) + 0.25*VDifferenceFromPrevious(abff)")
c = FrameEvaluate(c,"global diffb = 0.50*YDifferenceFromPrevious(atff) + 0.25*UDifferenceFromPrevious(atff) + 0.25*VDifferenceFromPrevious(atff)")
crop(c,0,0,16,16)
SelectRangeEvery(%d,%d,0)
`;
