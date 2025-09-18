#pragma once

//#include "V8dataModel/FaceInstance.h"
#include "V8Tree/instance.h"
#include "Util/TextureId.h"

namespace RBX {

	extern const char* const sSky;
	class Sky : public DescribedCreatable<Sky, Instance, sSky>
	{
	public:
		TextureId skyUp;
		TextureId skyLf;
		TextureId skyRt;
		TextureId skyBk;
		TextureId skyFt;
		TextureId skyDn;
		bool drawCelestialBodies;
	private:
		int numStars;
	public:
		Sky();
		int getNumStars() const { return numStars; }
		void setNumStars(int value);

        void setSkyboxUp(const TextureId&  texId); 
        void setSkyboxLf(const TextureId&  texId); 
        void setSkyboxRt(const TextureId&  texId); 
        void setSkyboxBk(const TextureId&  texId); 
        void setSkyboxDn(const TextureId&  texId); 
        void setSkyboxFt(const TextureId&  texId);
        const TextureId&  getSkyboxUp() const { return skyUp; }
        const TextureId&  getSkyboxLf() const { return skyLf; }
        const TextureId&  getSkyboxRt() const { return skyRt; }
        const TextureId&  getSkyboxBk() const { return skyBk; }
        const TextureId&  getSkyboxDn() const { return skyDn; }
        const TextureId&  getSkyboxFt() const { return skyFt; }
       
		static Reflection::PropDescriptor<Sky, int> prop_StarCount;
		static Reflection::PropDescriptor<Sky, TextureId> prop_SkyUp;
		static Reflection::PropDescriptor<Sky, TextureId> prop_SkyLf;
		static Reflection::PropDescriptor<Sky, TextureId> prop_SkyRt;
		static Reflection::PropDescriptor<Sky, TextureId> prop_SkyBk;
        static Reflection::PropDescriptor<Sky, TextureId> prop_SkyFt;
		static Reflection::PropDescriptor<Sky, TextureId> prop_SkyDn;
		static Reflection::BoundProp<bool> prop_CelestialBodiesShown;
	};
}