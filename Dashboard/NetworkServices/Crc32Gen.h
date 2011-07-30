#pragma once

#include <basetsd.h>

//! Generates a 32-bit cyclic redundancy check using a lookup table generated
//! from a given polynomial (the standard IEEE one is used by default)
class Crc32Generator
{
private:

	//! Lookup table for crc32 algorithm
	UINT32 table[256];

	//! Used by InitTable to flip the bits of an int
	UINT32 Reflect(UINT32 ref, char ch);

public:
	//! Polynomial used by IEEE
	static const UINT32 IEEEpoly = 0x04C11DB7;

	Crc32Generator(const UINT32 polynomial = IEEEpoly);

	//! Renerates the lookup table based on a different polynomial
	void RegenerateTable(const UINT32 polynomial);

	/*!
	\brief Get a crc32 checksum of a buffer of data
	\param buff The buffer to calculate the CRC from
	\param len The length of the buffer, in bytes
	\return The crc32 hash of the buffer
	*/	
	UINT32 GenerateChecksum(char* buff, size_t len);
};