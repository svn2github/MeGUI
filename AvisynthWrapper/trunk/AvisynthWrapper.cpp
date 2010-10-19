// modified by dimzon, renamed to AvisynthWrapper for futher independent development
// avisynth redirecter dll modified by Inc.
// Original by MobileHackerz http://www.nurs.or.jp/~calcium/

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <io.h>
#include <fcntl.h>
#include "internal.h"
#include "avisynth.h"
#include <windows.h>


typedef __int64 int64_t;
#include "avisynthdll.h"

#define MAX_CLIPS  1024
#define ERRMSG_LEN 1024


typedef struct tagSafeStruct
{
	char err[ERRMSG_LEN];
	IScriptEnvironment* env;
	AVSValue* res;
	PClip clp;
	HMODULE dll;
} SafeStruct;

extern "C" {
	__declspec(dllexport) int __stdcall dimzon_avs_init(SafeStruct** ppstr, char *func ,char *arg, AVSDLLVideoInfo *vi, int* originalPixelType, int* originalSampleType, char *cs);
	__declspec(dllexport) int __stdcall dimzon_avs_init_2(SafeStruct** ppstr, char *func ,char *arg, AVSDLLVideoInfo *vi, int* originalPixelType, int* originalSampleType, char *cs);
	__declspec(dllexport) int __stdcall dimzon_avs_destroy(SafeStruct** ppstr);
	__declspec(dllexport) int __stdcall dimzon_avs_getlasterror(SafeStruct* pstr, char *str,int len);
	__declspec(dllexport) int __stdcall dimzon_avs_getvframe(SafeStruct* pstr, void *buf, int stride, int frm );
	__declspec(dllexport) int __stdcall dimzon_avs_getaframe(SafeStruct* pstr, void *buf, __int64 start, __int64 count);
	__declspec(dllexport) int __stdcall dimzon_avs_getintvariable(SafeStruct* pstr, const char* name , int* result);
}


/*new implementation*/

int __stdcall dimzon_avs_getintvariable(SafeStruct* pstr, const char* name , int* result)
{
	try
	{
		pstr->err[0] = 0;
		try
		{
			AVSValue var = pstr->env->GetVar(name);
			if(var.Defined())
			{
				if(!var.IsInt())
				{
					strncpy_s(pstr->err, ERRMSG_LEN, "Variable is not Integer", _TRUNCATE);
					return -2;
				}
				*result = var.AsInt();
				return 0;
			}
			else
			{
				return 999; // Signal "Not defined"
			}
		}
		catch(AvisynthError err)
		{
			strncpy_s(pstr->err, ERRMSG_LEN, err.msg, _TRUNCATE);
			return -1;
		}
	}
	catch(IScriptEnvironment::NotFound)
	{
		return 666; // Signal "Not Found"
	}
}

int __stdcall dimzon_avs_getaframe(SafeStruct* pstr, void *buf, __int64 start, __int64 count)
{
	try
	{
		pstr->clp->GetAudio(buf,start,count,pstr->env);
		pstr->err[0] = 0;
		return 0;
	}
	catch(AvisynthError err)
	{
		strncpy_s(pstr->err, ERRMSG_LEN, err.msg, _TRUNCATE);
		return -1;
	}
}


int __stdcall dimzon_avs_getvframe(SafeStruct* pstr, void *buf, int stride, int frm )
{
	try
	{
		PVideoFrame f = pstr->clp->GetFrame(frm, pstr->env);
		if(buf && stride)
		{
			pstr->env->BitBlt((BYTE*)buf, stride, f->GetReadPtr(), f->GetPitch(),
							  f->GetRowSize(), f->GetHeight());
		}
		pstr->err[0] = 0;
		return 0;
	}
	catch(AvisynthError err)
	{
		strncpy_s(pstr->err, ERRMSG_LEN, err.msg, _TRUNCATE);
		return -1;
	}
}


int __stdcall dimzon_avs_getlasterror(SafeStruct* pstr, char *str,int len)
{
	strncpy_s(str,len,pstr->err,len-1);
	return (int)strlen(str);
}

int __stdcall dimzon_avs_destroy(SafeStruct** ppstr)
{
	if(!ppstr)
	{
		return 0;
	}

	SafeStruct* pstr = *ppstr;
	if(!pstr)
	{
		return 0;
	}

	if(pstr->clp)
	{
		pstr->clp = NULL;
	}

	if(pstr->res)
	{
		delete pstr->res;
		pstr->res = NULL;
	}

	if(pstr->env)
	{
		delete pstr->env;
		pstr->env = NULL;
	}

	if(pstr->dll)
	{
		FreeLibrary(pstr->dll);
	}

	free(pstr);
	*ppstr = NULL;
	return 0;
}

int __stdcall dimzon_avs_init(SafeStruct** ppstr, char *func ,char *arg, AVSDLLVideoInfo *vi, int* originalPixelType, int* originalSampleType, char *cs)
{
	SafeStruct* pstr = ((SafeStruct*)malloc(sizeof(SafeStruct)));
	*ppstr = pstr;
	memset(pstr,0,sizeof(SafeStruct));

	pstr->dll = LoadLibrary("avisynth.dll");
	if(!pstr->dll)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,"Cannot load avisynth.dll",_TRUNCATE);
		return 1;
	}

	IScriptEnvironment* (* CreateScriptEnvironment)(int version) = (IScriptEnvironment*(*)(int)) GetProcAddress(pstr->dll, "CreateScriptEnvironment");
	if(!CreateScriptEnvironment)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,"Cannot load CreateScriptEnvironment",_TRUNCATE);
		return 2;
	}

	pstr->env = CreateScriptEnvironment(AVISYNTH_INTERFACE_VERSION);

	if (pstr->env == NULL)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,"Required Avisynth 2.5",_TRUNCATE);
		return 3;
	}

	try
	{
		AVSValue arg(arg);
		AVSValue res = pstr->env->Invoke(func, AVSValue(&arg, 1));
		if (!res.IsClip()) {
			strncpy_s(pstr->err, ERRMSG_LEN, "The script's return was not a video clip.",_TRUNCATE);
			return 4;
		}
		pstr->clp = res.AsClip();
		VideoInfo inf  = pstr->clp->GetVideoInfo();
		VideoInfo infh = pstr->clp->GetVideoInfo();

		if (inf.HasVideo())
		{
			*originalPixelType =  inf.pixel_type;

			if ( strcmp("RGB24", cs)==0 && (!inf.IsRGB24()) )
			{
				res = pstr->env->Invoke("ConvertToRGB24", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();

				if(!infh.IsRGB24())
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to RGB24",_TRUNCATE);
					return	5;
				}
			}

			if ( strcmp("RGB32", cs)==0 && (!inf.IsRGB32()) )
			{
				res = pstr->env->Invoke("ConvertToRGB32", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();

				if(!infh.IsRGB32()) {
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to RGB32",_TRUNCATE);
					return 5;
				}
			}

			if ( strcmp("YUY2", cs)==0 && (!inf.IsYUY2()) )
			{
				res = pstr->env->Invoke("ConvertToYUY2", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();
				if(!infh.IsYUY2())
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to YUY2",_TRUNCATE);
					return 5;
				}
			}

			if ( strcmp("YV12", cs)==0 && (!inf.IsYV12()) )
			{
				res = pstr->env->Invoke("ConvertToYV12", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();
				if(!infh.IsYV12())
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to YV12",_TRUNCATE);
					return 5;
				}
			}
		}

		if (inf.HasAudio())
		{
			*originalSampleType = inf.SampleType();
			if( *originalSampleType != SAMPLE_INT16)
			{
				res = pstr->env->Invoke("ConvertAudioTo16bit", res);
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();
				if(infh.SampleType() != SAMPLE_INT16)
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert audio to 16bit",_TRUNCATE);
					return 6;
				}
			}
		}

		inf = pstr->clp->GetVideoInfo();
		if (vi != NULL) {
			vi->width   = inf.width;
			vi->height  = inf.height;
			vi->raten   = inf.fps_numerator;
			vi->rated   = inf.fps_denominator;
			vi->aspectn = 0;
			vi->aspectd = 1;
			vi->interlaced_frame = 0;
			vi->top_field_first  = 0;
			vi->num_frames = inf.num_frames;
			vi->pixel_type = inf.pixel_type;

			vi->audio_samples_per_second = inf.audio_samples_per_second;
			vi->num_audio_samples        = inf.num_audio_samples;
			vi->sample_type              = inf.sample_type;
			vi->nchannels                = inf.nchannels;
		}

		pstr->res = new AVSValue(res);

		pstr->err[0] = 0;
		return 0;
	}
	catch(AvisynthError err)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,err.msg,_TRUNCATE);
		return 999;
	}
}

int __stdcall dimzon_avs_init_2(SafeStruct** ppstr, char *func ,char *arg, AVSDLLVideoInfo *vi, int* originalPixelType, int* originalSampleType, char *cs)
{
// same as dimzon_avs_init() but without the fix audio output at 16 bit. New for AviSynth v2.5.7
	SafeStruct* pstr = ((SafeStruct*)malloc(sizeof(SafeStruct)));
	*ppstr = pstr;
	memset(pstr,0,sizeof(SafeStruct));

	pstr->dll = LoadLibrary("avisynth.dll");
	if(!pstr->dll)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,"Cannot load avisynth.dll",_TRUNCATE);
		return 1;
	}

	IScriptEnvironment* (* CreateScriptEnvironment)(int version) = (IScriptEnvironment*(*)(int)) GetProcAddress(pstr->dll, "CreateScriptEnvironment");
	if(!CreateScriptEnvironment)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,"Cannot load CreateScriptEnvironment",_TRUNCATE);
		return 2;
	}

	pstr->env = CreateScriptEnvironment(AVISYNTH_INTERFACE_VERSION);

	if (pstr->env == NULL)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,"Required Avisynth 2.5",_TRUNCATE);
		return 3;
	}

	try
	{
		AVSValue arg(arg);
		AVSValue res = pstr->env->Invoke(func, AVSValue(&arg, 1));
		if (!res.IsClip()) {
			strncpy_s(pstr->err, ERRMSG_LEN, "The script's return was not a video clip.",_TRUNCATE);
			return 4;
		}
		pstr->clp = res.AsClip();
		VideoInfo inf  = pstr->clp->GetVideoInfo();
		VideoInfo infh = pstr->clp->GetVideoInfo();

		if (inf.HasVideo())
		{
			*originalPixelType =  inf.pixel_type;

			if ( strcmp("RGB24", cs)==0 && (!inf.IsRGB24()) )
			{
				res = pstr->env->Invoke("ConvertToRGB24", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();

				if(!infh.IsRGB24())
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to RGB24",_TRUNCATE);
					return	5;
				}
			}

			if ( strcmp("RGB32", cs)==0 && (!inf.IsRGB32()) )
			{
				res = pstr->env->Invoke("ConvertToRGB32", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();

				if(!infh.IsRGB32()) {
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to RGB32",_TRUNCATE);
					return 5;
				}
			}

			if ( strcmp("YUY2", cs)==0 && (!inf.IsYUY2()) )
			{
				res = pstr->env->Invoke("ConvertToYUY2", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();
				if(!infh.IsYUY2())
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to YUY2",_TRUNCATE);
					return 5;
				}
			}

			if ( strcmp("YV12", cs)==0 && (!inf.IsYV12()) )
			{
				res = pstr->env->Invoke("ConvertToYV12", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				infh = pstr->clp->GetVideoInfo();
				if(!infh.IsYV12())
				{
					strncpy_s(pstr->err, ERRMSG_LEN,"Cannot convert video to YV12",_TRUNCATE);
					return 5;
				}
			}

		}

		inf = pstr->clp->GetVideoInfo();
		if (vi != NULL) {
			vi->width   = inf.width;
			vi->height  = inf.height;
			vi->raten   = inf.fps_numerator;
			vi->rated   = inf.fps_denominator;
			vi->aspectn = 0;
			vi->aspectd = 1;
			vi->interlaced_frame = 0;
			vi->top_field_first  = 0;
			vi->num_frames = inf.num_frames;
			vi->pixel_type = inf.pixel_type;

			vi->audio_samples_per_second = inf.audio_samples_per_second;
			vi->num_audio_samples        = inf.num_audio_samples;
			vi->sample_type              = inf.sample_type;
			vi->nchannels                = inf.nchannels;
		}

		pstr->res = new AVSValue(res);

		pstr->err[0] = 0;
		return 0;
	}
	catch(AvisynthError err)
	{
		strncpy_s(pstr->err, ERRMSG_LEN,err.msg,_TRUNCATE);
		return 999;
	}
}
