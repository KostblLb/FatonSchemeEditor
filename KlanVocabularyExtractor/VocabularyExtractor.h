// VocabularyExtractor.h

#pragma once
using namespace System;
using namespace System::Collections::Generic;
using namespace Shared;

//extracts VocTheme lists from these vocabularies:
//KLAN,
//
namespace VocabularyExtractor {
	public ref class Extractor
	{
	private:
		List<VocTheme^>^ _themes;
	public:
		Extractor() {
			_themes = nullptr;
		}
	public:
		//clr methods
		void ParseKlanVocabulary(String^% path);

		List<VocTheme^>^ Themes() { return _themes; }
	};
}
