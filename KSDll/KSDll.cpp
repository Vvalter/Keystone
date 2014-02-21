// KSDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KSDll.h"

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

KSDLL_API VOID SetMousePosition(INT x, INT y) 
{
	MouseMovement.mi.dwFlags = MOUSEEVENTF_MOVE;
	MouseMovement.mi.dx = x;
	MouseMovement.mi.dy = y;
	 
	SendInput(1, &MouseMovement, sizeof(INPUT));
}

static LRESULT CALLBACK Wrapper(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam); 
	}
	
	PKBDLLHOOKSTRUCT hookstruct = (PKBDLLHOOKSTRUCT) lparam;
	switch (wparam)
	{
		case WM_KEYDOWN:
			break;
		case WM_KEYUP:
			if (hookstruct->vkCode == VK_SPACE) {
				SetMousePosition(10, 100);
				callback(code, wparam, lparam);
			}
			break;
		case WM_SYSKEYDOWN:
			break;
		case WM_SYSKEYUP:
			break;
	}


	//callback(code, wparam, lparam);
	
	return CallNextHookEx(NULL, code, wparam, lparam);
}