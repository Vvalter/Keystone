// KSDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KSDll.h"

KSDLL_API  int KSDLLadd(int a, int b)
{
	return (int) handle;
}

KSDLL_API int SetHookCallback(HOOKPROC callback, INT hookID) 
{
	callbacks[hookID+1] = callback;
	SetWindowsHookEx(hookID, callback, handle, 0);
	return 0;
}
