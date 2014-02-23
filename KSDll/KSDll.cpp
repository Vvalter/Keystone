// KSDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KSDll.h"

/* Keyboard Hook */
KSDLL_API HHOOK InstallHook(FUNC f) 
{
	func = f;
	// TODO return null and see if KSDllWrapper.InstallHook throws Exception
	hook = SetWindowsHookEx(WH_KEYBOARD_LL, HookFunc, handle, 0);
	return hook;
}
KSDLL_API BOOL UninstallHook()
{
	return UnhookWindowsHookEx(hook);
}

/* Window Managment */
KSDLL_API VOID SetResolution(INT x, INT y)
{
	res_x = x;
	res_y = y;
}

/* Input Managment */
KSDLL_API VOID SetMousePosition(INT x, INT y) 
{
	MouseMovement.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
	MouseMovement.mi.dx = ConvertX(x);
	MouseMovement.mi.dy = ConvertY(y);
	 
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

/* Board Managment */
KSDLL_API VOID SetNumEnemy(INT num)
{
	enemies = num;
}
KSDLL_API VOID SetNumFriendly(INT num)
{
	friendly = num;
}
KSDLL_API VOID SetNumCards(INT num)
{
	cards = num;
}

static LRESULT CALLBACK HookFunc(INT code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	}

	func(code, wparam, lparam);

	/*PKBDLLHOOKSTRUCT hookstruct = (PKBDLLHOOKSTRUCT)lparam;
	if (wparam != WM_KEYUP && hookstruct->vkCode != VK_SPACE)
	{
		return CallNextHookEx(NULL, code, wparam, lparam);
	}

	const int d = 15;
	switch (hookstruct->vkCode)
	{
	case VK_LEFT: 
	case 0x48:
		(*current_pos)--;
		break;
	case VK_DOWN:
	case 0x4A:
		level++;
		break;
	case VK_UP:
	case 0x4B:
		level--;
		break;
	case VK_RIGHT:
	case 0x4C:
		(*current_pos)++;
		break;
	case VK_RETURN:
		SetMousePosition(END_X, END_Y);
		return 0;
	case VK_SPACE:
		PressMouse(wparam == WM_KEYDOWN);
		return CallNextHookEx(NULL, code, wparam, lparam);
	}
	// Keep position_y in bounds
	if (level < ENEMY_HERO)
	{
		level = FRIENDLY_CARDS;
	}

	if (level > FRIENDLY_CARDS)
	{
		level = ENEMY_HERO;
	}

	switch (level)
	{
	case ENEMY_HERO:
		current_pos = &tmp;
		SetMousePosition(ENEMY_HERO_X, ENEMY_HERO_Y);
		break;
	case ENEMY_MOBS:
		current_pos = &enemy_pos;
	case FRIENDLY_MOBS:
		current_pos = &enemy_pos;
	case FRIENDLY_HERO:
		current_pos = &hero_pos;
		SetMousePosition(ENEMY_HERO_X, ENEMY_HERO_Y);
	case FRIENDLY_CARDS:
		current_pos = &card_pos;
	}*/

	return CallNextHookEx(NULL, code, wparam, lparam);
}