// KlanVocabularyExtractor.h

#pragma once
using namespace System;
using namespace System::Collections::Generic;

namespace KlanVocabularyExtractor {

	public ref struct VocTheme {
		String^ name;			//knclass name
		List<VocTheme^>^ parents;	
		List<VocTheme^>^ children;
		VocTheme() {
			name = nullptr;
			parents = gcnew List<VocTheme^>();
			children = gcnew List<VocTheme^>();
		}
		bool Equals(VocTheme^ other) {
			return this->name->Equals(other->name);
		}
	};

	public ref struct VocEntry {
		String^ word;			//unicode string representing the word
		VocTheme^ wordClass;	//unicode string representing the word's class
	};

	public ref class Extractor
	{
	private:
		List<VocEntry^>^ _entries;
		List<VocTheme^>^ _themes;
	public:
		Extractor() {
			_entries = nullptr;
			_themes = nullptr;
		}
		Extractor(String^% path) {
			Extractor();
			this->ParseVocabulary(path);
		}
	public:
		//clr methods
		void ParseVocabulary(String^% path);
		//List<VocEntry^>^ Entries() { return _entries; }
		List<VocTheme^>^ Themes() { return _themes; }
	};
}
