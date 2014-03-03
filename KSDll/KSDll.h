#define KSDLL_API extern "C" __declspec(dllexport)

#include <vector>
#include "pHash.h"

using namespace std;

// The type of possible callbacks
typedef VOID (*FUNC) (BOOL, DWORD);

// Handle to this dll
HINSTANCE handle = NULL;

/* Keyboard Hook */
static FUNC funcKey, funcCBT;
static HHOOK hookKey, hookCBT;
static LRESULT CALLBACK HookKey(INT code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK HookCBT(INT code, WPARAM wparam, LPARAM lparam);

KSDLL_API HHOOK InstallHook(INT id, FUNC f);
KSDLL_API BOOL UninstallHook(INT id);

/* Input Managment */
INPUT MouseMovement;

KSDLL_API VOID SetMousePosition(INT x, INT y);
KSDLL_API VOID MoveMouse(INT dx, INT dy);
KSDLL_API VOID PressMouse(BOOL down);

/* Window Managment */
static RECT rect;

KSDLL_API LONG GetWindowX(HWND hWnd);
KSDLL_API LONG GetWindowY(HWND hWnd);
KSDLL_API LONG GetWindowWidth(HWND hWnd);
KSDLL_API LONG GetWindowHeight(HWND hWnd);

/* Image recognition */
#define HASH uint8_t*
#define HASHFUNC ph_mh_imagehash
#define HASHCMP ph_hammingdistance2
/*#define HASH ulong64
#define HASHFUNC ph_dct_imagehash
#define HASHCMP ph_hamming_distance*/

static vector<HASH> hashes;
static vector<PSTR> names;
KSDLL_API INT InitializeDatabase(PSTR dir);
KSDLL_API VOID ClearDatabase();
KSDLL_API PSTR RecognizeImage(PSTR img);
//KSDLL_API BOOL WriteToFile(PSTR file);