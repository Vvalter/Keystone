// KSDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KSDll.h"

KSDLL_API  int KSDLLadd(int a, int b)
{
	return 0;
}

KSDLL_API HHOOK SetKeyboardCallback(FUNC cb) 
{
	callback = cb;
	hook = SetWindowsHookEx(WH_KEYBOARD_LL, Wrapper, handle, 0);
	return hook;
}

KSDLL_API BOOL RemoveHook() 
{
	return UnhookWindowsHookEx(hook);
}

LRESULT CALLBACK Wrapper(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	}
	
	callback(code, wparam, lparam);
	
	return CallNextHookEx(NULL, code, wparam, lparam);
}