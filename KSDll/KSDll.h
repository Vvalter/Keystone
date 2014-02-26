#define KSDLL_API extern "C" __declspec(dllexport)

// The type of possible callbacks
typedef VOID (*FUNC) (BOOL, DWORD);

// Handle to this dll
HINSTANCE handle = NULL;

/* Keyboard Hook */
static HHOOK hook = NULL;
static FUNC func = NULL;
static LRESULT CALLBACK HookFunc(INT code, WPARAM wparam, LPARAM lparam);

KSDLL_API HHOOK InstallHook(FUNC f);
KSDLL_API BOOL UninstallHook();

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