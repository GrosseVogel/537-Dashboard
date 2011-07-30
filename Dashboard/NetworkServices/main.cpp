#include "Crc32Gen.h"

Crc32Gen generator;

extern "C"
{
	__declspec(dllexport) UINT32 GetCrc(char* buff, size_t len)
	{
		return generator.GenerateChecksum(buff, len);
	}
}