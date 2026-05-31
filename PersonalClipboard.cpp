// PersonalClipboard.cpp
// compile with: /D_UNICODE /DUNICODE /DWIN32 /D_WINDOWS /c
// Sample code sources:
// Window creation:
// https://learn.microsoft.com/en-us/cpp/windows/walkthrough-creating-windows-desktop-applications-cpp?view=msvc-170
// Context menu creation:
// https://www.equestionanswers.com/vcpp/popup-menu-context-menu.php

#include <windows.h>
#include <stdlib.h>
#include <string.h>
#include <tchar.h>
#include <WinUser.h>
#include <stdio.h>
 /* Context Menu IDs */
#define IDM_CONTEXT_LINE   1000
#define IDM_CONTEXT_RECTAN 1001
#define IDM_CONTEXT_CIRCLE 1002
#define IDM_CONTEXT_CLOSE   1003
/* Context Menu items enumeration */
enum MenuItems { Line, Square, Circle, Close };

// Global variables

// The main window class name.
static TCHAR szWindowClass[] = _T("DesktopApp");

// The string that appears in the application's title bar.
static TCHAR szTitle[] = _T("Personal Clipboard Application");

// Stored instance handle for use in Win32 API calls such as FindResource
HINSTANCE hInst;

// Forward declarations of functions included in this code module:
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

int WINAPI WinMain(
    _In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPSTR     lpCmdLine,
    _In_ int       nCmdShow
)
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(wcex.hInstance, IDI_APPLICATION);
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszMenuName = NULL;
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(wcex.hInstance, IDI_APPLICATION);

    if (!RegisterClassEx(&wcex))
    {
        MessageBox(NULL,
                   _T("Call to RegisterClassEx failed!"),
                   _T("Personal Clipboard"),
                   NULL);

        return 1;
    }

    // Store instance handle in our global variable
    hInst = hInstance;

    // The parameters to CreateWindowEx explained:
    // WS_EX_OVERLAPPEDWINDOW : An optional extended window style.
    // szWindowClass: the name of the application
    // szTitle: the text that appears in the title bar
    // WS_OVERLAPPEDWINDOW: the type of window to create
    // CW_USEDEFAULT, CW_USEDEFAULT: initial position (x, y)
    // 500, 100: initial size (width, height)
    // NULL: the parent of this window
    // NULL: this application does not have a menu bar
    // hInstance: the first parameter from WinMain
    // NULL: not used in this application
    HWND hWnd = CreateWindowEx(
        WS_EX_OVERLAPPEDWINDOW,
        szWindowClass,
        szTitle,
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT,
        500, 100,
        NULL,
        NULL,
        hInstance,
        NULL
    );

    if (!hWnd)
    {
        MessageBox(NULL,
                   _T("Call to CreateWindowEx failed!"),
                   _T("Personal Clipboard"),
                   NULL);

        return 1;
    }

    // The parameters to ShowWindow explained:
    // hWnd: the value returned from CreateWindowEx
    // nCmdShow: the fourth parameter from WinMain
    //ShowWindow(hWnd,
    //           nCmdShow);
    //UpdateWindow(hWnd);
    // Popup the context menu
    POINT pt;
    BOOL bReturn = GetCursorPos(&pt);
    HMENU hMenuPopup = CreatePopupMenu();
    AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_LINE, _T("Line"));
    AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_RECTAN, _T("Square"));
    AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_CIRCLE, _T("Circle"));
    AppendMenu(hMenuPopup, MF_SEPARATOR, 0, NULL);
    AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_CLOSE, _T("Close"));

    if (bReturn == 0) //If GetCursorPos() encountered error
    {
        pt.x = 0;
        pt.y = 0;
    }
    ClientToScreen(hWnd, &pt);
    TrackPopupMenu(hMenuPopup,
                   TPM_LEFTALIGN | TPM_RIGHTBUTTON,
                   pt.x, pt.y, 0, hWnd, NULL);
    DestroyMenu(hMenuPopup);

    // Main message loop:
    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return (int)msg.wParam;
}

//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_PAINT    - Paint the main window
//  WM_DESTROY  - post a quit message and return
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    PAINTSTRUCT ps;
    HDC hdc;
    TCHAR greeting[] = _T("Hello, Windows desktop!");

    switch (message)
    {
    case WM_PAINT:
    {
        hdc = BeginPaint(hWnd, &ps);

        // Here your application is laid out.
        // For this introduction, we just print out "Hello, Windows desktop!"
        // in the top left corner.
        TextOut(hdc,
                5, 5,
                greeting, _tcslen(greeting));
        // Popup the context menu
        POINT pt;
        BOOL bReturn = GetCursorPos(&pt);
        HMENU hMenuPopup = CreatePopupMenu();
        //AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_LINE, _T("Line"));
        //AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_RECTAN, _T("Square"));
        //AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_CIRCLE, _T("Circle"));
        //AppendMenu(hMenuPopup, MF_SEPARATOR, 0, NULL);
        //AppendMenu(hMenuPopup, MF_STRING, IDM_CONTEXT_CLOSE, _T("Close2"));

        if(bReturn == 0) //If GetCursorPos() encountered error
        {
            pt.x = 0;
            pt.y = 0;
        }
        ClientToScreen(hWnd, &pt);
        TrackPopupMenu(hMenuPopup,
                       TPM_LEFTALIGN | TPM_RIGHTBUTTON,
                       pt.x, pt.y, 0, hWnd, NULL);
        DestroyMenu(hMenuPopup);
        // End application-specific layout section.

        EndPaint(hWnd, &ps);
        //break;
    }
    case WM_COMMAND:
    {
        if (wParam >= IDM_CONTEXT_LINE && wParam <= IDM_CONTEXT_CLOSE)
        {
            int menuItemIndex = wParam - IDM_CONTEXT_LINE;
            const char* menuItem = "";
            LPCWSTR msg = _T("");
            switch (menuItemIndex)
            {
                case MenuItems::Line:
                msg = _T("Clicked on Line");
                break;
            case MenuItems::Square:
                msg = _T("Clicked on Square");
                break;
            case MenuItems::Circle:
                msg = _T("Clicked on Circle");
                break;
            case MenuItems::Close:
                msg = _T("Clicked on Close");
                ExitProcess(0);
                break;
            }
            MessageBoxW(hWnd, (LPCWSTR)msg, _T("Popup Menu"), MB_OK);
        }
        DefWindowProc(hWnd, message, wParam, lParam);
        break;
    }
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
        break;
    }

    return 0;
}