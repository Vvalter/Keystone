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
KSDLL_API INT InitializeDatabase(LPSTR dir)
{
	WIN32_FIND_DATAA fd;
	CHAR dirWithStar[MAX_PATH];
	strcpy(dirWithStar, dir);
	strcat(dirWithStar, "\\*");
	HANDLE hd = FindFirstFileA(dirWithStar, &fd);
	if (INVALID_HANDLE_VALUE == hd)
	{
		return -1;
	}
	do
	{
		// Check Extension
		int len = strlen(fd.cFileName);
		if (len < 5) continue;
		if (strcmp(fd.cFileName + len - 4, ".bmp") != 0) continue;
		if (!(fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY))
		{
			PSTR full_name = (PSTR)::CoTaskMemAlloc(MAX_PATH + 1);
			strcpy(full_name, dir);
			strcat(full_name, "\\");
			strcat(full_name, fd.cFileName);

			int N;
			HASH hash = HASHFUNC(full_name, N);
			/*HASH hash;
			HASHFUNC(full_name, hash);*/
			hashes.push_back(hash);
			names.push_back(full_name);
		}
	} while (FindNextFileA(hd, &fd) != 0);

	if (GetLastError() != ERROR_NO_MORE_FILES)
	{
		return -1;
	}
	FindClose(hd);
	return (INT)hashes.size();
}
KSDLL_API VOID ClearDatabase()
{
	for (HASH hash : hashes)
	{
		//free(hash);
	}
	for (PSTR name : names)
	{
		CoTaskMemFree(name);
	}
	hashes.clear();
	names.clear();
}
KSDLL_API PSTR RecognizeImage(PSTR img)
{
	int N;
	HASH img_hash = HASHFUNC(img, N);
	/*HASH img_hash;
	HASHFUNC(img, img_hash);*/
	int best = 0;
	double best_score = 1000;
	
	for (size_t i = 0; i < hashes.size(); i++)
	{
		double score = HASHCMP(img_hash, N, hashes[i], N);
		//double score = HASHCMP(img_hash, hashes[i]);
		if (score < best_score)
		{
			best = i;
			best_score = score;
		}
	}
	return names[best];
}

/*KSDLL_API BOOL WriteToFile(PSTR file)
{
	FILE *f = fopen(file, "w");
	
	for (int i = 0; i < hashes.size(); i++)
	{
		fprintf(f, "%d<>")
	}

}*/