#include <boost/type_traits/is_floating_point.hpp>

#pragma once
namespace RBX {

	namespace Text
	{
		enum Font   {FONT_LEGACY, FONT_ARIAL, FONT_ARIALBOLD, FONT_SOURCESANS, FONT_SOURCESANSBOLD, FONT_SOURCESANSLIGHT, FONT_SOURCESANSITALIC, FONT_LAST};
		// Font drawing params - copied from G3D
		enum XAlign {XALIGN_RIGHT, XALIGN_LEFT, XALIGN_CENTER};

		enum YAlign {YALIGN_TOP, /*YALIGN_BASELINE,*/ YALIGN_CENTER, YALIGN_BOTTOM};


	}
}