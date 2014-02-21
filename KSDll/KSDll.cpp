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
	MouseMovement.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
	MouseMovement.mi.dx = x;
	MouseMovement.mi.dy = y;
	 
	SendInput(1, &MouseMovement, sizeof(INPUT));
}

KSDLL_API VOID MoveMouse(INT dx, INT dy) 
{
	MouseMovement.mi.dwFlags = MOUSEEVENTF_MOVE;
	MouseMovement.mi.dx = dx;
	MouseMovement.mi.dy = dy;
	 
	SendInput(1, &MouseMovement, sizeof(INPUT));
}
KSDLL_API VOID PressMouse(BOOL down) 
{
	MouseMovement.mi.dwFlags = (down) ? (MOUSEEVENTF_LEFTDOWN) : (MOUSEEVENTF_LEFTUP);
	SendInput(1, &MouseMovement, sizeof(INPUT));
}
static LRESULT CALLBACK Wrapper(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam); 
	}
	
	PKBDLLHOOKSTRUCT hookstruct = (PKBDLLHOOKSTRUCT) lparam; 
	if (wparam != WM_KEYUP && hookstruct->vkCode != VK_SPACE) 
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	} 

	const int d = 15;
	switch (hookstruct->vkCode) 
	{
		case VK_LEFT:
		case 0x48:
			MoveMouse(-d, 0);
			break;
		case VK_DOWN:
		case 0x4A:
			MoveMouse(0, d);
			break;
		case VK_UP:
		case 0x4B:
			MoveMouse(0, -d);
			break;
		case VK_RIGHT:
		case 0x4C:
			MoveMouse(d, 0);
			break;
		case VK_SPACE:
			//PressMouse(wparam == WM_KEYDOWN);
			SetMousePosition(10, 100);
			break;
	}
	
	return CallNextHookEx(NULL, code, wparam, lparam);
}