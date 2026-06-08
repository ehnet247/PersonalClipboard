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