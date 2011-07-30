#include "Crc32Gen.h"
#include <cstring>

Crc32Generator::Crc32Generator(const UINT32 polynomial)
{
	RegenerateTable(polynomial);
}

inline UINT32 Crc32Generator::Reflect(UINT32 ref, char ch)
{
	UINT32 value = 0;

	// Swap bit 0 for bit 7 
	// bit 1 for bit 6, etc. 
	for(int i = 1; i < (ch + 1); ++i) 
	{ 
		if(ref & 1) 
			value |= 1 << (ch - i); 

		ref >>= 1; 
	} 
	return value; 
}

void Crc32Generator::RegenerateTable(const UINT32 polynomial)
{
	//Reset the table first
	memset(table, 0, sizeof(UINT32) * 256);
	
	// 256 values representing ASCII character codes. 
    for(int i = 0; i <= 0xFF; i++) 
    { 
		table[i] = Reflect(i, 8) << 24; 
        for (int j = 0; j < 8; j++) 
		{
			table[i] = (table[i] << 1) ^ (table[i] & (1 << 31) ? polynomial : 0); 
		}
        table[i] = Reflect(table[i], 32); 
	} 
}

UINT32 Crc32Generator::GenerateChecksum(char* buff, size_t len)
{
	register UINT32 crc = 0xFFFFFFFF;

    for( ; len; --len, ++buff)
    {
		crc = (table[((crc) ^ ((unsigned char)*buff)) & 0xff] ^ ((crc) >> 8));
    }

	return ~crc;
}