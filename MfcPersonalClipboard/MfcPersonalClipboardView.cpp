
// MfcPersonalClipboardView.cpp : implémentation de la classe CMfcPersonalClipboardView
//

#include "pch.h"
#include "framework.h"
// SHARED_HANDLERS peuvent être définis dans les gestionnaires d'aperçu, de miniature
// et de recherche d'implémentation de projet ATL, et permettent le partage de code de document avec ce projet.
#ifndef SHARED_HANDLERS
#include "MfcPersonalClipboard.h"
#endif

#include "MfcPersonalClipboardDoc.h"
#include "MfcPersonalClipboardView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMfcPersonalClipboardView

IMPLEMENT_DYNCREATE(CMfcPersonalClipboardView, CView)

BEGIN_MESSAGE_MAP(CMfcPersonalClipboardView, CView)
	// Commandes d'impression standard
	ON_COMMAND(ID_FILE_PRINT, &CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, &CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, &CMfcPersonalClipboardView::OnFilePrintPreview)
	ON_WM_CONTEXTMENU()
	ON_WM_RBUTTONUP()
END_MESSAGE_MAP()

// construction/destruction de CMfcPersonalClipboardView

CMfcPersonalClipboardView::CMfcPersonalClipboardView() noexcept
{
	// TODO: ajoutez ici du code de construction

}

CMfcPersonalClipboardView::~CMfcPersonalClipboardView()
{
}

BOOL CMfcPersonalClipboardView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: changez ici la classe ou les styles Window en modifiant
	//  CREATESTRUCT cs

	return CView::PreCreateWindow(cs);
}

// dessin de CMfcPersonalClipboardView

void CMfcPersonalClipboardView::OnDraw(CDC* /*pDC*/)
{
	CMfcPersonalClipboardDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;

	// TODO: ajoutez ici le code de dessin pour les données natives
}


// impression de CMfcPersonalClipboardView


void CMfcPersonalClipboardView::OnFilePrintPreview()
{
#ifndef SHARED_HANDLERS
	AFXPrintPreview(this);
#endif
}

BOOL CMfcPersonalClipboardView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// préparation par défaut
	return DoPreparePrinting(pInfo);
}

void CMfcPersonalClipboardView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: ajoutez une initialisation supplémentaire avant l'impression
}

void CMfcPersonalClipboardView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: ajoutez un nettoyage après l'impression
}

void CMfcPersonalClipboardView::OnRButtonUp(UINT /* nFlags */, CPoint point)
{
	ClientToScreen(&point);
	OnContextMenu(this, point);
}

void CMfcPersonalClipboardView::OnContextMenu(CWnd* /* pWnd */, CPoint point)
{
#ifndef SHARED_HANDLERS
	theApp.GetContextMenuManager()->ShowPopupMenu(IDR_POPUP_EDIT, point.x, point.y, this, TRUE);
#endif
}


// diagnostics de CMfcPersonalClipboardView

#ifdef _DEBUG
void CMfcPersonalClipboardView::AssertValid() const
{
	CView::AssertValid();
}

void CMfcPersonalClipboardView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CMfcPersonalClipboardDoc* CMfcPersonalClipboardView::GetDocument() const // la version non Debug est inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CMfcPersonalClipboardDoc)));
	return (CMfcPersonalClipboardDoc*)m_pDocument;
}
#endif //_DEBUG


// gestionnaires de messages de CMfcPersonalClipboardView
