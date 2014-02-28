#define KSDLL_API extern "C" __declspec(dllexport)

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