
// MfcPersonalClipboardView.h : interface de la classe CMfcPersonalClipboardView
//

#pragma once


class CMfcPersonalClipboardView : public CView
{
protected: // création à partir de la sérialisation uniquement
	CMfcPersonalClipboardView() noexcept;
	DECLARE_DYNCREATE(CMfcPersonalClipboardView)

// Attributs
public:
	CMfcPersonalClipboardDoc* GetDocument() const;

// Opérations
public:

// Substitutions
public:
	virtual void OnDraw(CDC* pDC);  // substitué pour dessiner cette vue
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);

// Implémentation
public:
	virtual ~CMfcPersonalClipboardView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Fonctions générées de la table des messages
protected:
	afx_msg void OnFilePrintPreview();
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint point);
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // version Debug dans MfcPersonalClipboardView.cpp
inline CMfcPersonalClipboardDoc* CMfcPersonalClipboardView::GetDocument() const
   { return reinterpret_cast<CMfcPersonalClipboardDoc*>(m_pDocument); }
#endif

