// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the KSDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// KSDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
/*#ifdef KSDLL_EXPORTS
#define KSDLL_API __declspec(dllexport)
#else
#define KSDLL_API __declspec(dllimport)
#endif*/
#define KSDLL_API extern "C" __declspec(dllexport)

#define WH_NUM (WH_MAX-WH_MIN+1)

typedef VOID (*FUNC) (INT, WPARAM, LPARAM);

HINSTANCE handle = NULL;
INPUT MouseMovement;

/* Keyboard Hook */
static HHOOK hook = NULL;
static FUNC func = NULL;
static LRESULT CALLBACK HookFunc(INT code, WPARAM wparam, LPARAM lparam);
KSDLL_API HHOOK InstallHook(FUNC f);
KSDLL_API BOOL UninstallHook();

static LRESULT CALLBACK Wrapper(INT code, WPARAM wparam, LPARAM lparam);

/* Window Managment */
static INT res_x = 1920, res_y = 1080;
KSDLL_API VOID SetResolution(INT x, INT y);
inline INT ConvertX(int x)
{
	return (x * 65535) / res_x;
}
inline INT ConvertY(int y)
{
	return (y * 65535) / res_y;
}

/* Input Managment */
KSDLL_API VOID SetMousePosition(INT x, INT y);
KSDLL_API VOID MoveMouse(INT dx, INT dy);
KSDLL_API VOID PressMouse(BOOL down);

/* Board Managment */
static INT enemies, friendly, cards;
KSDLL_API VOID SetNumEnemy(INT num);
KSDLL_API VOID SetNumFriendly(INT num);
KSDLL_API VOID SetNumCards(INT num);

// TODO 
static const INT END_X = 1550, END_Y = 490;
static const INT ENEMY_HERO_X = 960, ENEMY_HERO_Y = 200;
static const INT FRIENDLY_HERO_X = 960, FRIENDLY_HERO_Y = 850;
static const INT ABILITY_X = 1130, ABILITY_Y = 830;
static const INT MIDDLE_X = 960;
static const INT MOB_WIDTH = 140;

static enum YLEVEL {
	ENEMY_HERO = 0, ENEMY_MOBS = 1, FRIENDLY_MOBS = 2, FRIENDLY_HERO = 3, FRIENDLY_CARDS = 4
};

static INT level = FRIENDLY_CARDS;
static INT enemy_pos, friendly_pos, hero_pos, card_pos, tmp;

static PINT current_pos = &friendly_pos;