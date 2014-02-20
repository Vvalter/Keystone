// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the KSDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// KSDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
/*#ifdef KSDLL_EXPORTS
#define KSDLL_API __declspec(dllexport)
#else
#define KSDLL_API __declspec(dllimport)
#endif*/
#define KSDLL_API extern "C" __declspec(dllexport)

#define WH_NUM (WH_MAX-WH_MIN+1)
HINSTANCE handle = NULL;

static HOOKPROC callbacks[WH_NUM];

KSDLL_API int KSDLLadd(int a, int b);
KSDLL_API int SetHookCallback(HOOKPROC callback, INT hookID);