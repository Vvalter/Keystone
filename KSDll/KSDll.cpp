#include "stdafx.h"
#include "KSDll.h"

/* Keyboard Hook */
static LRESULT CALLBACK HookFunc(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	}

	BOOL down = (wparam == WM_KEYDOWN || wparam == WM_SYSKEYDOWN);
	DWORD vkc = ((PKBDLLHOOKSTRUCT)lparam)->vkCode;
	func(down, vkc);

	return CallNextHookEx(NULL, code, wparam, lparam);
}

KSDLL_API HHOOK InstallHook(FUNC f) 
{
	func = f;
	hook = SetWindowsHookEx(WH_KEYBOARD_LL, HookFunc, handle, 0);
	return hook;
}

KSDLL_API BOOL UninstallHook()
{
	return UnhookWindowsHookEx(hook);
}

/* Input Managment */
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

/* Window Managment */
KSDLL_API LONG GetWindowX(HWND hWnd)
{
	GetWindowRect(hWnd, &rect);
	return rect.left;
}
KSDLL_API LONG GetWindowY(HWND hWnd)
{
	GetWindowRect(hWnd, &rect);
	return rect.top;
}
KSDLL_API LONG GetWindowWidth(HWND hWnd)
{
	GetClientRect(hWnd, &rect);
	return rect.right - rect.left;
}
KSDLL_API LONG GetWindowHeight(HWND hWnd)
{
	GetClientRect(hWnd, &rect);
	return rect.bottom - rect.top;
}