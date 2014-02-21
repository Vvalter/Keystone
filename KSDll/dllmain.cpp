// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"

extern HINSTANCE handle;
extern INPUT MouseMovement;
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		handle = hModule;
		MouseMovement.type = INPUT_MOUSE;
		MouseMovement.mi.time = 0;
		MouseMovement.mi.mouseData = 0;

		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}