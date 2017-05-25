// ֳכאגםûי DLL-פאיכ.

#include "stdafx.h"

#include "Klan/KNKernel.h"
#include "KlanVocabularyExtractor.h"


using System::Runtime::InteropServices::Marshal;

namespace KlanVocabularyExtractor {

	bool smt() {
		return true;
	}
	void Extractor::ParseVocabulary(String^% path) {
		//auto entries = gcnew List<VocEntry^>();
		auto themes = gcnew List<VocTheme^>();
		
		char* cstr = (char*)Marshal::StringToHGlobalAnsi(path).ToPointer();

		IKNManager* man = GenerateAPIManager();
		int err = man->SetPath(cstr);
		err = man->Load();
		if (err != 0) {
			printf_s("%s %s", "failed to load vocab on", cstr);
			man->Release();
			return;
		}
		
		IKNThemeList* kthemes = man->GetThemesTable();
		for (int i = 0; i < kthemes->GetCount(); i++) {
			IKNTheme* ktheme = kthemes->GetTheme(i);
			VocTheme^ theme = gcnew VocTheme();
			theme->name = gcnew String(ktheme->GetName());
			IKNTheme* kparent = ktheme->GetFirstParent();
			while (kparent) {
				VocTheme^ parent = nullptr;
				for each (VocTheme^ th in themes)
				{
					//for each kparent there's always a parent in themes
					if (th->name->Equals(gcnew String(kparent->GetName()))) {
						parent = th;
						break;
					}
				}
				theme->parents->Add(parent);
				parent->children->Add(theme);
				kparent = ktheme->GetNextParent();
			}
			themes->Add(theme);
		}

		_themes = themes;
		man->Release();


		Marshal::FreeHGlobal((IntPtr)cstr);
	}
}