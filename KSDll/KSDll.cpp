#include "stdafx.h"
#include "KSDll.h"

/* Hooks */
static LRESULT CALLBACK HookKey(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	}

	BOOL down = (wparam == WM_KEYDOWN || wparam == WM_SYSKEYDOWN);
	DWORD vkc = ((PKBDLLHOOKSTRUCT)lparam)->vkCode;
	funcKey(down, vkc);


	return CallNextHookEx(NULL, code, wparam, lparam);
}

static LRESULT CALLBACK HookCBT(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	}

	funcCBT(true, wparam);

	return CallNextHookEx(NULL, code, wparam, lparam);
}

KSDLL_API HHOOK InstallHook(INT id, FUNC f) 
{
	switch (id)
	{
	case WH_KEYBOARD_LL:
		funcKey = f;
		hookKey = SetWindowsHookEx(id, HookKey, handle, 0);
		return hookKey;
	case WH_CBT:
		funcCBT = f;
		hookCBT = SetWindowsHookEx(id, HookCBT, handle, 0);
		return hookCBT;
	default:
		return NULL;
	}
}
KSDLL_API BOOL UninstallHook(INT id)
{
	switch (id)
	{
	case WH_KEYBOARD_LL:
		return UnhookWindowsHookEx(hookKey);
	case WH_CBT:
		return UnhookWindowsHookEx(hookCBT);
	default:
		return false;
	}
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

/* Image recognition */
KSDLL_API BOOL InitializeDatabase(const char* dir)
{
	WIN32_FIND_DATAA fd;
	HANDLE hd = FindFirstFileA(dir, &fd);
	if (INVALID_HANDLE_VALUE == hd)
	{
		return false;
	}
	do
	{
		if (fd.dwFileAttributes & FILE_ATTRIBUTE_NORMAL)
		{
			// Handle fd.cFileName
			int N;
			uint8_t *hash = ph_mh_imagehash(fd.cFileName, N);
			hashes.push_back(hash);
			char* name = (char*)malloc(strlen(dir) + 1);
			strcpy(name, dir);
			names.push_back(name);
		}
	} while (FindNextFileA(hd, &fd) != 0);
	if (GetLastError() != ERROR_NO_MORE_FILES)
	{
		return false;
	}
	FindClose(hd);
	return TRUE;
}
KSDLL_API VOID ClearDatabase()
{
	for (uint8_t *hash : hashes)
	{
		free(hash);
	}
	for (char* name : names)
	{
		free(name);
	}
	hashes.clear();
	names.clear();
}
KSDLL_API const char* RecognizeImage(const char* img)
{
	int N;
	uint8_t *img_hash = ph_mh_imagehash(img, N);

	
	for (uint8_t *hash : hashes)
	{
	    
	}
	return NULL;
}