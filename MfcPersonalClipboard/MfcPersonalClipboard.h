
// MfcPersonalClipboard.h : fichier d'en-tête principal de l'application MfcPersonalClipboard
//
#pragma once

#ifndef __AFXWIN_H__
	#error "incluez 'pch.h' avant d'inclure ce fichier pour PCH"
#endif

#include "resource.h"       // symboles principaux


// CMfcPersonalClipboardApp :
// Consultez MfcPersonalClipboard.cpp pour l'implémentation de cette classe
//

class CMfcPersonalClipboardApp : public CWinAppEx
{
public:
	CMfcPersonalClipboardApp() noexcept;


// Substitutions
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

// Implémentation
	BOOL  m_bHiColorIcons;

	virtual void PreLoadState();
	virtual void LoadCustomState();
	virtual void SaveCustomState();

	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CMfcPersonalClipboardApp theApp;
