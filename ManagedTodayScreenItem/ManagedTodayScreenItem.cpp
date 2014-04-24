#include <windows.h>
#include <todaycmn.h>
#include <aygshell.h>
#pragma comment (lib, "aygshell.lib")
#include "ManagedTodayScreenItem.h"
 
//====================================================================
static HINSTANCE gbl_hInst;

static HANDLE gbl_hProcess;
static HWND gbl_hWndManaged;
static DWORD gbl_dwHeight;

//----------------------------------------------------------------------
static LRESULT DoQueryRefreshCacheMain(HWND hWnd, WPARAM wParam, LPARAM lParam)
{
	// The today screen sends this message every 2 seconds or so. If the
	// managed part of our today screen item has reported that its
	// height has changed we'll change the height of the native part to
	// match.
    TODAYLISTITEM *ptli = (TODAYLISTITEM *)wParam;

	if (ptli->cyp != gbl_dwHeight)
	{
		// The managed part has changed height, so change the
		// height of the native part and return TRUE to indicate
		// that we've changed it.
		ptli->cyp = gbl_dwHeight;
		return TRUE;
	}

	// We haven't changed height etc so return FALSE to
	// report no changes.
	return FALSE;
}

static LRESULT DoClearCacheMain(HWND hWnd, WPARAM wParam, LPARAM lParam) 
{
	// We don't cache data, so nothing to do here
    return 0;
}

static LRESULT DoSize(HWND hWnd, WPARAM wParam, LPARAM lParam)
{
	// The today screen item has been resized. If the managed
	// part of our item has been initialised we need to resize
	// it to cover the entire client area.
	if (gbl_hWndManaged)
	{
		RECT rcBounds;
		GetClientRect(hWnd, &rcBounds);
		SetWindowPos(gbl_hWndManaged, NULL, 0, 0, rcBounds.right - rcBounds.left, rcBounds.bottom - rcBounds.top, SWP_NOZORDER);
	}
	
	return 0;
}

static LRESULT DoPaintMain(HWND hWnd, WPARAM wParam, LPARAM lParam)
{
	// The (native) today screen item window has requested a repaint
	// so we'll draw the watermark image onto it, just in case the
	// managed part of our plugin isn't currently initialised or has
	// crashed. Typically the managed part will completely cover the
	// client area of our window, so this background won't be seen.
	TODAYDRAWWATERMARKINFO dwi;
	dwi.hdc = (HDC)wParam;
	GetClientRect(hWnd, &dwi.rc);
	dwi.hwnd = hWnd;

	SendMessage(GetParent(hWnd), TODAYM_DRAWWATERMARK, 0, (LPARAM)&dwi);

	return 0;
}

static LRESULT DoPaintMainManaged(HWND hWnd, WPARAM wParam, LPARAM lParam)
{
	// The (managed) today screen item window has requested a repaint
	// of it's background. So we'll draw the watermark image onto it.
	TODAYDRAWWATERMARKINFO dwi;

	HWND hWndDest = (HWND)lParam;
	HDC hdcDest = GetDC(hWndDest);

	RECT rcBounds;
	GetClientRect(hWnd, &rcBounds);

	HBITMAP hBmp = CreateCompatibleBitmap(hdcDest, rcBounds.right - rcBounds.left, rcBounds.bottom - rcBounds.top);
	HDC hdcSrc = CreateCompatibleDC(hdcDest);
	SelectObject(hdcSrc, hBmp);

	dwi.hdc = hdcSrc;
	GetClientRect(hWnd, &dwi.rc);
	dwi.hwnd = hWnd;
	SendMessage(GetParent(hWnd), TODAYM_DRAWWATERMARK, 0, (LPARAM)&dwi);

	MapWindowPoints(hWndDest, hWnd, (LPPOINT)&rcBounds, 2);
	BitBlt(hdcDest, 0, 0, rcBounds.right - rcBounds.left, rcBounds.bottom - rcBounds.top,
		hdcSrc, rcBounds.left, rcBounds.top, SRCCOPY);
	
	DeleteDC(hdcSrc);
	DeleteObject(hBmp);

	ReleaseDC(hWndDest, hdcDest);

	return 0;
}

static LRESULT DoParentManaged(HWND hWnd, WPARAM wParam, LPARAM lParam)
{
	// The managed part of the today screen item has been initialised and
	// is telling us it's window handle and desired height.
	gbl_dwHeight = (DWORD)wParam;
	gbl_hWndManaged = (HWND)lParam;

	// So reparent the managed window so that it is non full screen and
	// becomes a child of the native part of our today screen plugin.
	DWORD dwStyle = GetWindowLong(gbl_hWndManaged, GWL_STYLE);
	dwStyle &= ~WS_POPUP;
	dwStyle |= WS_CHILD;
	SetWindowLong(gbl_hWndManaged, GWL_STYLE, dwStyle);

	SetParent(gbl_hWndManaged, hWnd);

	RECT rcBounds;
	GetClientRect(hWnd, &rcBounds);
	SetWindowPos(gbl_hWndManaged, NULL, 0, 0, rcBounds.right - rcBounds.left, rcBounds.bottom - rcBounds.top, SWP_NOZORDER);

	return 0;
}

// static int index = -1;

static LRESULT TodayWndProc(HWND hWnd, UINT wMsg, WPARAM wParam, LPARAM lParam)
{
	switch (wMsg)
	{
		case 1234:
			return DoParentManaged(hWnd, wParam, lParam);

		case 1235:
			return DoPaintMainManaged(hWnd, wParam, lParam);

		case WM_ERASEBKGND:
			return DoPaintMain(hWnd, wParam, lParam);

		case WM_SIZE:
			return DoSize(hWnd, wParam, lParam);

		case WM_TODAYCUSTOM_CLEARCACHE:
			return DoClearCacheMain(hWnd, wParam, lParam);

		case WM_TODAYCUSTOM_QUERYREFRESHCACHE:
			return DoQueryRefreshCacheMain(hWnd, wParam, lParam);

/*
		case WM_TODAYCUSTOM_RECEIVEDSELECTION:
			if (wParam == VK_UP)
				index = 2;
			else if (wParam == VK_DOWN)
				index = 0;
			return TRUE; // [CF] - TODO - return FALSE if we want to skip the selection, i.e. when height == 0

		case WM_TODAYCUSTOM_LOSTSELECTION:
			index = -1;
			break;

		case WM_TODAYCUSTOM_USERNAVIGATION:
			if (wParam == VK_UP)
			{
				if (index > 0)
				{
					index--;
					return TRUE;
				}
			}
			else if (wParam == VK_DOWN)
			{
				if (index < 2)
				{
					index++;
					return TRUE;
				}
			}
		
			return FALSE;
*/

		default:
			return DefWindowProc(hWnd, wMsg, wParam, lParam);
	}

	return 0;
}

//======================================================================
// Entry point called to create today screen item
HWND APIENTRY InitializeCustomItem(TODAYLISTITEM *ptli, HWND hWndParent)
{
	SHELLEXECUTEINFO se;
	HWND hWnd;

    // See if not enabled.
    if (!ptli->fEnabled)
        return FALSE;

	// Create the child widow for our today screen plugin. The initial
	// height is zero pixels high, this will be updated in response to
	// the today screen host sending the WM_TODAYCUSTOM_QUERYREFRESHCACHE
	// message
	gbl_dwHeight = 0;
	hWnd = CreateWindow(TODAYWND_CLASS, NULL, WS_VISIBLE | WS_CHILD, 
                         0, 0, GetSystemMetrics(SM_CXSCREEN), 0,
                         hWndParent, NULL, gbl_hInst, 0);
	// [CF] - TODO - should we store hWnd in ptli->hwndCustom?

	// Recored in the registry the window handle for our native
	// window. The managed part of the today screen plugin
	// will read this key in order to determine which window
	// it needs to communicate with.
	HKEY hKey;
	TCHAR szKey[MAX_PATH];

	// [CF] - TODO - handle this failing....
	_stprintf(szKey, _T("Software\\Microsoft\\Today\\Items\\%s"), ptli->szName);
	if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, szKey, 0, 0, &hKey) == ERROR_SUCCESS)
	{
		DWORD dwValue = (DWORD)hWnd;
		RegSetValueEx(hKey, _T("NativeHandle"), 0, REG_DWORD, (LPBYTE)&dwValue, sizeof(dwValue));
		RegCloseKey(hKey);
	}

	// Launch the managed application that hosts our
	// .NET CF based today screen items. The executable should
	// be located in the same folder as our native DLL.
	memset(&se, 0, sizeof(se));
    se.cbSize = sizeof(se);
    se.hwnd = hWndParent;

	// [CF] - TODO - use CreateProcess and not ShellExecuteEx so we don't get message boxes if this fails...
	TCHAR szHost[MAX_PATH];
	GetModuleFileName(gbl_hInst, szHost, sizeof(szHost) / sizeof(szHost[0]));
	_tcscpy(_tcsrchr(szHost, '\\') + 1, _T("ManagedTodayScreenItemHost.exe"));
	se.lpFile = szHost;
    se.lpVerb = _T("open");
    se.lpParameters = NULL;
    ShellExecuteEx(&se);

	gbl_hProcess = se.hProcess;

    return hWnd;
}

//======================================================================
// Entry point called to process options dialog box (we don't currently support one)
BOOL CALLBACK CustomItemOptionsDlgProc(HWND hWnd, UINT wMsg, WPARAM wParam, LPARAM lParam)
{
	switch (wMsg)
	{
		case WM_INITDIALOG:
			{
			SHINITDLGINFO shidi = {0};
			shidi.dwMask = SHIDIM_FLAGS;
			shidi.hDlg = hWnd;
			shidi.dwFlags = SHIDIF_DONEBUTTON | SHIDIF_SIZEDLGFULLSCREEN;
			SHInitDialog(&shidi);
			}
			break;

		case WM_COMMAND:
			switch (LOWORD(wParam))
			{
			case IDOK:
				EndDialog(hWnd, LOWORD(wParam));
				break;
			}
			break;

		default:
			return FALSE;
	}

	return TRUE;
}

//====================================================================
// DllMain - DLL initialization entry point
//
BOOL WINAPI DllMain(HANDLE hinstDLL, DWORD dwReason, LPVOID lpvReserved)
{
    switch (dwReason)
	{
		case DLL_PROCESS_ATTACH:
			gbl_hInst = (HINSTANCE)hinstDLL;
			DisableThreadLibraryCalls(gbl_hInst);

			// Register the today screen item window class.
			WNDCLASS wc;
			memset (&wc, 0, sizeof (wc));
			wc.style = CS_HREDRAW | CS_VREDRAW;
			wc.lpfnWndProc = TodayWndProc;
			wc.hInstance = gbl_hInst;
			wc.lpszClassName = TODAYWND_CLASS;
			wc.hbrBackground = (HBRUSH)(COLOR_WINDOW+1);
			RegisterClass(&wc);
			break;
 
		case DLL_PROCESS_DETACH:
			// Unregister our window class, otherwise we can fail
			// to reload
			UnregisterClass(TODAYWND_CLASS, gbl_hInst);
			break;
    }

    return TRUE;
}
