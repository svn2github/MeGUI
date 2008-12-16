/*
 * Avisynth DLL definitions
 * MobileHackerz http://www.nurs.or.jp/~calcium/
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#ifndef AVISYNTHDLL_H
#define AVISYNTHDLL_H

typedef struct AVSDLLVideoInfo {
	// Video
 	int width;
	int height;
	int raten;
	int rated;
	int aspectn;
	int aspectd;
	int interlaced_frame;
	int top_field_first;
	int num_frames;
	int pixel_type;

	// Audio
	int audio_samples_per_second;
	int sample_type;
	int nchannels;
	int num_audio_frames;
	int64_t num_audio_samples;
} AVSDLLVideoInfo;

typedef struct AVSDLLVideoPlane {
	int width;
	int height;
	int pitch;
	BYTE *data;
} AVSDLLVideoPlane;


typedef struct FRAME {
	int *data;
} FRAME;



typedef struct AVSDLLVideoFrame {
	AVSDLLVideoPlane plane[3];
} AVSDLLVideoFrame;

typedef struct AVSDLLAudioPos {
	int64_t start;
	int64_t count;
} AVSDLLAudioPos;


#endif /* AVISYNTHDLL_H */
