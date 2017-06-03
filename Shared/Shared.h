// Shared.h

#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace Shared {
	public ref class VocTheme {
	public: String^ name;			//class name
			bool root;
			List<VocTheme^>^ children;
			List<VocTheme^>^ parents;
			VocTheme() {
				name = nullptr;
				root = true;
				parents = gcnew List<VocTheme^>();
				children = gcnew List<VocTheme^>();
			}
			VocTheme(String^% s) : VocTheme() {
				name = s;
			}
			bool Equals(VocTheme^ other) {
				return this->name->Equals(other->name);
			}
	};
}
