// testConsole.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "testConsole.h"

#include "orange.h"

#include <assert.h>
#include <bitset> //for bitset
#include <conio.h> //for getch()
#include <map>
#include <memory> //for auto_ptr
#include <strstream>
#include <stdio.h>
#include <vector>

#include <algorithm>
#include <comutil.h>
#include <comdef.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define ASSERT_ALWAYS(bCond)		if(!bCond) { __asm{int 3}; } //will assert, even in Release build :-)

#define Q(x) (#x)
#define CALL_TEST_METHOD(methodName)	\
		printf(Q(methodName));			\
		printf("\r\n");						\
		methodName();							\
		printf("\r\n");

#pragma warning (disable:4482)

// The one and only application object

CWinApp theApp;

using namespace std;

#ifdef UNICODE
typedef std::wstring tstring;
#define tsprintf_s swprintf_s
#else
typedef std::string tstring;
#define tsprintf_s sprintf_s
#endif

struct lessString2
{
   bool operator()(std::string strX,std::string strY) const
   { 
      bool bLess = false;
      if(strcmp(strX.c_str(),strY.c_str()) < 0)
         bLess = true;
      return bLess; 
   }
};

typedef std::map<std::string, std::string, lessString2> _StlMap_t;

#define AMTLEN 15

tstring raw_double_to_text(double dAmt)
//=============================================================================
{
//	const long AMTLEN = 15;
	TCHAR buf[AMTLEN+1];

	tsprintf_s(buf, sizeof(buf) / sizeof(buf[0]), _T("%f"), dAmt); //SR. - HostMapper will handle converting with decimal places for currency.

	return buf;
}

void test_raw_double_to_text()
{
	printf("dAmount: 1.48 = ");
	double dAmount = 1.48;
	tstring tstr = raw_double_to_text(dAmount);

	_tprintf(tstr.c_str());
}

void testFormatF()
{
	double dWide = 10000000.0000001;
//	const long AMTLEN = 15;
	char buf[AMTLEN+1];
	sprintf_s(buf, sizeof(buf), "%15f", dWide); //SR. - HostMapper will handle converting with decimal places for currency.

	printf("dWide: '10000000.0000001'");
	printf(buf);
}


void copyMapMember(LPCSTR pszKey, const _StlMap_t& srcMsgMap, _StlMap_t& destMsgMap)
{
	_StlMap_t::const_iterator it = srcMsgMap.find(pszKey);
	if(it != srcMsgMap.end())
		destMsgMap[pszKey] = it->second.c_str();
}

void test_copyMapMember()
{
	vector<string> keys;
	keys.push_back("one");
	keys.push_back("two");
	keys.push_back("three");
	keys.push_back("four");

	int iVal = 1;

	LPCSTR pszNotCopy = "_DO_NOT_COPY";

	//populate the source map:
	vector<string> copiedKeys;
	_StlMap_t billMap;
	int iCount = 0;
	for(vector<string>::iterator it = keys.begin(); it != keys.end(); it++)
	{
		if(iCount > 0) //skip the first one - see that copyMapMember() can handle a missing member
		{
			const int iBufSize = 4;
			char buf[iBufSize];
			sprintf_s(buf, iBufSize, "%d", iVal++);

			billMap[*it] = buf;
			copiedKeys.push_back(*it);
		}
		iCount++;
	}

	billMap[pszNotCopy] = pszNotCopy;

	//use copyMapMember():
	_StlMap_t targetMap;
	for(vector<string>::iterator it = keys.begin(); it != keys.end(); it++)
	{
		copyMapMember((*it).c_str(), billMap, targetMap);
	}
	
	//check the results:
	if(targetMap.size() != copiedKeys.size())
		throw 1;
	for(vector<string>::iterator it = copiedKeys.begin(); it != copiedKeys.end(); it++)
	{
		_StlMap_t::iterator itSource = billMap.find(*it);
		if(itSource == billMap.end())
			throw 1;

		_StlMap_t::iterator itTarget = targetMap.find(*it);
		if(itTarget == targetMap.end())
			throw 1;

		string strSrcVal = itSource->second;
		string strTgtVal = itTarget->second;

		if(strSrcVal.compare(strTgtVal) != 0)
			throw 1;
	}
}

void test_virtual()
{
	{
		cout << "calling CBase::MyOverridablePublicMethod()" << endl;
		CFruitBase base;
		EFruitType type = base.MyOverridablePublicMethod();
		ASSERT_ALWAYS(type == EFruitType::BASE);

		type = base.CallPrivateMethod();
		ASSERT_ALWAYS(type == EFruitType::BASE);
	}
	{
		cout << "calling CFruit::MyOverridablePublicMethod()" << endl;
		CFruit fruit;
		EFruitType type = fruit.MyOverridablePublicMethod();
		ASSERT_ALWAYS(type == EFruitType::BASE);

		type = fruit.CallPrivateMethod();
		ASSERT_ALWAYS(type == EFruitType::BASE);
	}
	{
		cout << "calling COrange::MyOverridablePublicMethod()" << endl;
		COrange orange;
		EFruitType type = orange.MyOverridablePublicMethod();
		ASSERT_ALWAYS(type == EFruitType::ORANGE);

		type = orange.CallPrivateMethod();
		ASSERT_ALWAYS(type == EFruitType::ORANGE);
	}
}

COrange* createOrangePointer()
{
	return new COrange();
}

auto_ptr<COrange> createOrange()
{
	//COrange* pOrange = new COrange();
	auto_ptr<COrange> ptrOrange(createOrangePointer());
	return ptrOrange;
}

void useAutoPtrParam(auto_ptr<COrange>& ptrOrange)
{
	ptrOrange->MyOverridablePublicMethod();
}

void test_auto_ptr()
{
	//call a method that will create an object,
	//and rely on auto_ptr() to delete it:
	auto_ptr<COrange> ptrOrange = createOrange();
	EFruitType type = ptrOrange->MyOverridablePublicMethod();
	ASSERT_ALWAYS(type == EFruitType::ORANGE);

	//try copying the auto_ptr: the source should be NULL?
	auto_ptr<COrange> ptrOrange2 = ptrOrange; // = transfers ownership
	ASSERT_ALWAYS(ptrOrange.get() == NULL);
	ASSERT_ALWAYS(ptrOrange2.get() != NULL);
	ptrOrange2->MyOverridablePublicMethod();

	useAutoPtrParam(ptrOrange2);

	//we should NOT get a memory leak!

	//new COrange(); //xxx test a memory leak
}

void test_hex_to_bin()
{
	char* buffer = "0123456789ABCDEF";
	int buflen = strlen(buffer);

	const int iNumHexDigits = buflen != 0 ? buflen / 2  : 0;
	unsigned char aBin[200];
   for (int i = 0; i < iNumHexDigits; i++)
	{
		//xxx
		//Our input 'buffer' is a HEX string (not in binary).
		//So, we need to convert from hex string, into binary:
		DWORD tmp = 0;
		//sscanf(&buffer[i*2], "%02x", &tmp);
		sscanf_s(&buffer[i*2], "%02x", &tmp, sizeof(DWORD));

		aBin[i] = (unsigned char)tmp;
	}

	ASSERT_ALWAYS(FALSE); //debug break! - need to manually verify the binary
}

void stringToHexString(char const*const pcszIn, int iInLen, char* pszOut, int iOutLen)
{
	ASSERT_ALWAYS( (iInLen <= (iOutLen/2)) );

	strstream str(pszOut, iOutLen);
	for(int i = 0; i < iInLen; i++)
	{
		unsigned char c = pcszIn[i];

		//((ios_base)str) << 
		//std::cout << std::hex << (int)c;
		str << std::hex << (int)c;
	}
	str.flush();
}

void test_string_to_hex()
{
	const int MAX_MSG_SIZE = 255;

	const int iHexLen = MAX_MSG_SIZE * 2;
	char hexBuffer[iHexLen];

	const int iBinLen = MAX_MSG_SIZE;
	char binBuffer[iBinLen];

	//create some binary:
	int i = 0;
	for(char c = 'a'; c <= 'z'; c++)
	{
        std::bitset<sizeof(char) * 8> binary(c); //sizeof() counts bytes, not bits
        std::cout << "Letter: " << c << "\t";
        std::cout << "Hex: " << std::hex << (int)c << "\t";
        std::cout << "Binary: " << binary << std::endl;

		  binBuffer[i++] = c; //*(binary.to_string().c_str());
   }

	stringToHexString(binBuffer, iBinLen, hexBuffer, iHexLen); //convert to hex string for GenerateMAC()

	ASSERT_ALWAYS(FALSE); //debug break! - need to manually verify the binary
}

void test_string_length()
{
	string strEmpty(_T(""));
	ASSERT_ALWAYS(strEmpty.length() == 0);
}


//////////////////////////////////////////////
namespace LogOverLoad
{
	enum log_level {_LOG_DEBUG, _LOG_INFO };
	#define LOG_BUFFER_SIZE 1024

	void log_write(log_level lev, LPCTSTR lpszText)
	{
		std::cout << "log_write() - " << lpszText << endl;
	}

	void LogWrite( log_level level, LPCTSTR lpszText, ... )
	{
		char _buffer[LOG_BUFFER_SIZE]={0};

		va_list argptr;         
		va_start (argptr, lpszText );         
		_vsntprintf (_buffer, sizeof(_buffer), lpszText, argptr );         
		va_end (argptr);

		log_write(level, _buffer);
	}

	//=============================================================================
	void LogWrite(log_level level, LPCTSTR lpszFile, DWORD dwLine, LPCTSTR lpszText, ...)
	//=============================================================================
	{
		// NOTE: use of filename, and linenumber has been
		// deprecated and so they are not used by this function.
		// The prototype hasn't been updated to maintain compatibility 
		// with the 1000's of calls made to these functions.
		char buffer[LOG_BUFFER_SIZE]={0};

		//note: this function can be called by at least 2 kinds of client:
		//1. client that wants to call this method 
		//	(i.e. lpszFile is a filename, lpszText is a format string)
		//2. client that actually wants to call Write( bwac::log::log_level level, LPCTSTR lpszText, ... )
		//	(i.e. lpszFile is actually a format string, and dwLine and lpszText are the first 2 parameters)

		//LPCTSTR plszFormatString = lpszText; //case 1
		if(NULL != _tcschr(lpszFile, _T('%')))
		{
			//case 2 - lpszFile is actually a format string !
			//we can only safely process this kind of format string:
			//first param: %d, second param: %s
			//Anything else can cause a problem, because the number has already been cast to a DWORD
			const char* pszFmtNumber = _tcsstr(lpszFile, "%d");
			const char* pszFmtString = _tcsstr(lpszFile, "%s");
			const char* pszFmt3 = _tcsstr(pszFmtString + 1, "%"); //is there a 3rd param?

			if(pszFmtNumber && pszFmtString && pszFmtString > pszFmtNumber && !pszFmt3)
			{
				//plszFormatString = lpszFile;
				sprintf_s(buffer, LOG_BUFFER_SIZE, lpszFile, dwLine, lpszText);
				//note: any args in the ... are still lost .. to use them, we would need to mess about with va_list, which could cause problems if we ever upgrade from VS2005!
			}
			else
			{
				_tcscpy(buffer, lpszFile); //just output the raw format string, which we cannot process
			}
		}
		else
		{
			va_list argptr;         
			//va_start (argptr, plszFormatString );
			va_start (argptr, lpszText );
			_vsntprintf_s(buffer, sizeof(buffer), lpszText, argptr );         
			va_end (argptr);
		}

		log_write(level, buffer);	
	}
}

#define LOG LogOverLoad::LogWrite

void test_logging_overload()
{
	//old-style, correct overload:
	LOG( LogOverLoad::_LOG_INFO, "xxx - LOG macro - line before 2:");
	LOG( LogOverLoad::_LOG_INFO, __FILE__, __LINE__, "xxx - LOG macro - line with FILE - 666:%d abc:%s", 666, "abc");

	//incorrect overload:
	LOG( LogOverLoad::_LOG_INFO, "xxx - LOG macro - line before 1:");
	LOG( LogOverLoad::_LOG_INFO, "xxx - LOG macro - Service 666:%d is enabled:%s for this account", 666, TRUE ? "enabled" : "disabled");

	//crashing overload (crashes with new code for case 2):
	LOG( LogOverLoad::_LOG_INFO, "Amount [%d] formatted to [%s] ", 199000, "199000.00 IDR ");
	LOG( LogOverLoad::_LOG_INFO, "Amount [%lf] formatted to [%s] ", 199000, "199000.00 IDR ");
}

//////////////////////////////////////////////

void test_sprintf_preceding_zeros()
{
	const int kiBuffSize = 512;
	TCHAR buffer[kiBuffSize];

	int iBuffSize = sizeof(buffer)/sizeof(buffer[0]);

	if(iBuffSize != kiBuffSize)
	{
		cout << "test failed - iBuffSize: " << iBuffSize << " - kiBuffSize: " << kiBuffSize << endl;
		ASSERT_ALWAYS(FALSE); //fail !
	}

	tsprintf_s(buffer, iBuffSize, _T("%.2d"), 3);
	TCHAR* pszExpected = _T("03");
	if(_tcscmp(buffer, pszExpected) != 0)
	{
		cout << "test failed! - buffer: " << buffer << " - expected: " << pszExpected << endl;
		ASSERT_ALWAYS(FALSE); //fail !
	}
}

//////////////////////////////////////////////

void test_short_to_int()
{
	int iMinusOne = -1;
	short nMinusOne = iMinusOne;

	if(nMinusOne != iMinusOne)
		ASSERT_ALWAYS(FALSE);
}

//////////////////////////////////////////////

void test_detect_NFTFER640()
{
	tstring uniqueKey = _T("NetworkFundsTransfer_0010_Request");
	tstring first20 = uniqueKey.substr(0, 20);
	
	if (!(first20 == _T("NetworkFundsTransfer")))
	{
		ASSERT_ALWAYS(FALSE);
	}
}

//////////////////////////////////////////////

#define MAC_TRUNCATED_LENGTH 8

int upper(int c) //to avoid problem, if you include both <iostream> and <cctype> because <iostream> declares a version of toupper for locales
{
	return toupper((unsigned char)c);
}

void GenerateMAC(_bstr_t &mac)
{
	mac = "testMac1234";

	string str_mac_code = mac; //copy MAC so that we can manipulate it
	if(str_mac_code.length() > MAC_TRUNCATED_LENGTH)
	{
		str_mac_code = str_mac_code.substr(0, MAC_TRUNCATED_LENGTH);
	}
	std::transform(str_mac_code.begin(), str_mac_code.end(), str_mac_code.begin(), upper); //make upper case
	mac = str_mac_code.c_str();
}

void test_bstr_t()
{
	_bstr_t mac;
	GenerateMAC(mac);
	cout << "mac: " << mac << endl;

	string mac_string = mac;
	assert(mac_string == "TESTMAC1");
}

//////////////////////////////////////////////

void test_print_short()
{
	short nShort = -1;
	printf("-1 = %ld\n", (long)nShort);

	nShort = 2;
	printf("2 = %ld\n", (long)nShort);
}

//////////////////////////////////////////////
typedef BOOL (*PFISFALLBACKIMD)(LPCSTR);

void test_load_dll_and_call_method()
{
	HMODULE hm = ::LoadLibrary("MyDll.dll");

	BOOL val = FALSE;

	if (NULL != hm)
	{
		cout << "CMyClass::IsMyFun() - found MyDll.dll";

		PFISFALLBACKIMD pFun = (PFISFALLBACKIMD) ::GetProcAddress(hm, "IsMyFun");

		LPCSTR szIMD = "123456";

		if (pFun)
		{
			cout << "CMyClass::IsMyFun() - checking input %s";
			cout << szIMD;
			val = pFun(szIMD);
		}

		::FreeLibrary(hm);
	}

	ASSERT_ALWAYS(val == TRUE);
}

//////////////////////////////////////////////

//after CWA parse of MTF file :0)
void testBitShift()
{
	_int64 m_PrimaryBitmap = 0x8010202020202020;

	int Position = 35; //Track2

	//right-shift, so that the bit we are interested in is at 0x1
	//note: when we right-shift, the new LSB is filled with a '1' which is somewhat surprising (as 0x8... has LSB of '0')
	//I say left most bit is the LSB since we are little Endian (on Windows)
	_int64 shifted = m_PrimaryBitmap  >> (64-(Position));
	
	WORD wLow = LOWORD(shifted);

	ASSERT_ALWAYS(0x1 & wLow);
}

//////////////////////////////////////////////
void testMacroIncrement()
{
	__COUNTER__; //1?
	__COUNTER__; //2?
	__COUNTER__; //3?

	//note: you can turn on Precompiler output file, to see what these macros actually output.

	//note: this does not do what I want: 
	//_MY_COUNTER_START actually becomes __COUNTER__ rather than the value of __COUNTER__ !
	//I guess a Python or Powershell script would sort this out ...
#define _MY_COUNTER_START __COUNTER__

	const int iOne = (__COUNTER__ - _MY_COUNTER_START);
	const int iTwo = (__COUNTER__ - _MY_COUNTER_START);
}

//////////////////////////////////////////////

int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;

	// initialize MFC and print and error on failure
	if (!AfxWinInit(::GetModuleHandle(NULL), NULL, ::GetCommandLine(), 0))
	{
		// TODO: change error code to suit your needs
		_tprintf(_T("Fatal Error: MFC initialization failed\n"));
		nRetCode = 1;
	}
	else
	{
		CALL_TEST_METHOD(testFormatF)
		CALL_TEST_METHOD(test_raw_double_to_text)
		CALL_TEST_METHOD(test_copyMapMember)
		CALL_TEST_METHOD(test_virtual)
		CALL_TEST_METHOD(test_auto_ptr)
		CALL_TEST_METHOD(test_hex_to_bin)
		CALL_TEST_METHOD(test_string_to_hex)
		CALL_TEST_METHOD(test_string_length)
		CALL_TEST_METHOD(test_logging_overload)
		CALL_TEST_METHOD(test_sprintf_preceding_zeros)
		CALL_TEST_METHOD(test_short_to_int)
		CALL_TEST_METHOD(test_detect_NFTFER640)
		CALL_TEST_METHOD(test_print_short)
		CALL_TEST_METHOD(test_bstr_t)
		CALL_TEST_METHOD(test_load_dll_and_call_method)
		CALL_TEST_METHOD(testBitShift)
		CALL_TEST_METHOD(testMacroIncrement)
	}

	printf("Press any key to continue");
	_getch();

	return nRetCode;
}
