#include <string>
#include <tchar.h>
using namespace std;
//#pragma once
/* Context Menu IDs */
#define IDM_CONTEXT_LINE   1000
#define IDM_CONTEXT_RECTAN 1001
#define IDM_CONTEXT_CIRCLE 1002
#define IDM_CONTEXT_CLOSE   1003

struct MenuItem
{
	int Id;
	string Value = "oh!";
};

//struct MenuItemCollection
//{
//private:
//	list<MenuItem> m_MenuItems;
//public:
//	MenuItemCollection();
//MenuItemCollection::MenuItemCollection()
//{
//	m_MenuItems = { {IDM_CONTEXT_LINE, _T("Line")}, {IDM_CONTEXT_RECTAN, _T("Square")}, {IDM_CONTEXT_CIRCLE, _T("Circle")}, {IDM_CONTEXT_CLOSE, _T("Close")} };
//}
//	list<MenuItem> getMenuItems() const { return m_MenuItems; }
//};