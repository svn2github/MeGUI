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

module avireader;
private 
{
	import std.c.windows.windows;
	import std.stdio;
}

class AVIException : Exception
{
	this(string message)
	{
		super(message);
	}
}

unittest
{
	AVIFileInit();
	int pavifile;
	assert (!AVIFileOpen(&pavifile, "blank.avs", 0x20, 0));
	int pvideostream;
	assert (!AVIFileGetStream(pavifile, &pvideostream, 1935960438, 0));
	int pget = AVIStreamGetFrameOpen(pvideostream, 0);
	assert (pget);
	assert (AVIStreamGetFrame(pget, 0));
	assert (!AVIStreamGetFrameClose(pget));
	assert (!AVIStreamRelease(pvideostream));
	assert (!AVIFileRelease(pavifile));
	AVIFileExit();
	writefln("reading through blank.avs");
	readThroughAvi("blank.avs", null);
	writefln("Completed successfully");
	return 0;
}

void readThroughAvi(string aviFilename, void delegate(uint number, uint total) status)
{
	AviFile file;
	with (file = new AviFile())
	{
		openFile(aviFilename);
		for (int i = 0; i < framecount; i++)
		{
			if (status != null) status(i, framecount);
			getFrame(i);
		}
		if (status != null)
			status(framecount, framecount);
		close();
	}
}

class AviFile
{
	private 
	{
		int m_pAviFile;
		int m_pavistream;
		int m_pget;
		int m_framecount;
	}
	
	int framecount(int value) { return m_framecount = value; }
	int framecount() { return m_framecount; }
	
	public this() 
	{
		AVIFileInit();
	}

	public ~this()
	{
		close();
	}
	
	public void openFile(string aviFilename)
	{
		if (AVIFileOpen(&m_pAviFile, std.string.toStringz(aviFilename), 0x20, 0))
			throw new AVIException("Error calling AVIFileOpen");
		if (AVIFileGetStream(m_pAviFile, &m_pavistream, 1935960438, 0))
			throw new AVIException("Error calling AVIFileGetStream");
		m_pget = AVIStreamGetFrameOpen(m_pavistream, 0);
		if (!m_pget)
			throw new AVIException("Error calling AVIStreamGetFrameOpen");
		m_framecount = AVIStreamLength(m_pavistream);
	}
	
	public void getFrame(int framenumber)
	{
		AVIStreamGetFrame(m_pget, framenumber);
	}
	
	public void close()
	{
		if (m_pget)
		{
			AVIStreamGetFrameClose(m_pget);
			m_pget = 0;
		}
		if (m_pavistream)
		{
			AVIStreamRelease(m_pavistream);
			m_pavistream = 0;
		}
		if (m_pAviFile)
		{
			AVIFileRelease(m_pAviFile);
			m_pAviFile = 0;
		}
		AVIFileExit();
	}
	
}

struct AVISTREAMINFO
{
	DWORD		fccType;
	DWORD       fccHandler;
	DWORD       dwFlags;        /* Contains AVITF_* flags */
	DWORD		dwCaps;
	WORD		wPriority;
	WORD		wLanguage;
	DWORD       dwScale;
	DWORD       dwRate; /* dwRate / dwScale == samples/second */
	DWORD       dwStart;
	DWORD       dwLength; /* In units above... */
	DWORD		dwInitialFrames;
	DWORD       dwSuggestedBufferSize;
	DWORD       dwQuality;
	DWORD       dwSampleSize;
	RECT        rcFrame;
	DWORD		dwEditCount;
	DWORD		dwFormatChangeCount;
	WCHAR		szName[64];
}


extern (Windows)
{
	void AVIFileInit();
	void AVIFileExit();
	int AVIFileOpen(int* ppAviFile, LPCTSTR filename, UINT mode, int pclsidHandler);
	int AVIFileRelease(int pfile);
	int AVIFileGetStream(int pfile, int* ppavistream, DWORD fccType, LONG lParam);
	int AVIStreamRelease(int pavistream);
	int AVIStreamInfo(int pavi, AVISTREAMINFO* psi, int lSize);
	int AVIStreamGetFrameOpen(int pavi, int lpbiWanted);
	int AVIStreamGetFrameClose(int pget);
	int AVIStreamGetFrame(int pgf, int lPos);
	int AVIStreamLength(int pavi);
}