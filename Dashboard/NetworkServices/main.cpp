#include "Crc32Gen.h"

Crc32Generator generator;

extern "C"
{
	//! A DLL export of Crc32Gen.GenerateChecksum
	__declspec(dllexport) UINT32 GetCrc(char* buff, size_t len)
	{
		return generator.GenerateChecksum(buff, len);
	}
}