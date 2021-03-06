#pragma region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#pragma endregion

#include "VolumeApi.h"
#include "VideoDecoder.h"
#include "VideoEncoder.h"

int DecompressVolume(Stream* input, SliceWriter* output ) {
	auto decoder = std::shared_ptr<VideoDecoder>(new VideoDecoder());
	return decoder->Decode(input, output);
}

int DecompressSlices(Stream* input, SliceWriter* output, unsigned short index, unsigned short count ) {
	auto decoder = std::shared_ptr<VideoDecoder>(new VideoDecoder());
	return decoder->Decode(input, output, index, count);
}

int CompressVolume(SliceReader* input, Stream* output, unsigned short width, unsigned short height, const char* encoderName, const char* pixelFormat, const char* options, int bitrate) {

	auto encoder = std::shared_ptr<VideoEncoder>(new VideoEncoder(width, height));
	return encoder->Encode(input, output, encoderName, pixelFormat, options, bitrate);
}