#pragma once

#include <basetsd.h>

class Crc32Gen
{
protected:

	//Lookup table for crc32 algorithm
	UINT32 table[256];

	//Used by InitTable
	UINT32 Reflect(UINT32 ref, char ch);

	inline UINT32 Crc32Gen::UpdateCrc(char octet, UINT32 crc)
	{
		return (table[((crc) ^ ((unsigned char)octet)) & 0xff] ^ ((crc) >> 8));
	}

public:
	//Polynomials used by IEEE
	static const UINT32 IEEEpoly = 0x04C11DB7;
	static const size_t kCRCWidth = sizeof(UINT32);
	//Constructor
	Crc32Gen();
	Crc32Gen(const UINT32 polynomial);

	//Generate table based on a different polynomial
	void RegenerateTable(const UINT32 polynomial);

	//Get a crc32 checksum
	UINT32 GenerateChecksum(char* buff, size_t len);
};