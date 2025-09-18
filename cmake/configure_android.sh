(set -o igncr) 2>/dev/null && set -o igncr; # comment is needed on Windows to ignore this lines trailing \r
#!/bin/bash

ANDROID_API=21

CLIENT_DIR="$(dirname "${BASH_SOURCE[0]}")/.."
BUILD_DIR="$CLIENT_DIR/../build"

#GENERATOR="Eclipse CDT4 - Unix Makefiles"

GENERATOR="Eclipse CDT4 - Unix Makefiles"

case $(uname) in
    CYGWIN*)
        GENERATOR="Eclipse CDT4 - MinGW Makefiles"
        ;;
esac

# Make sure the following arrays' indices match in terms of
# values you want to be used together in loops.
# NOTE: You cannot assign lists to bash arrays; hence, the awkward code.
declare -a BUILD_DIRECTORIES || exit $?
BUILD_DIRECTORIES[0]=${BUILD_DIR}/release
BUILD_DIRECTORIES[1]=${BUILD_DIR}/noopt
BUILD_DIRECTORIES[2]=${BUILD_DIR}/debug

declare -a BUILD_MODES || exit $?
BUILD_MODES[0]="Release"
BUILD_MODES[1]="Noopt"
BUILD_MODES[2]="Debug"

COMMON_CMAKE_ARGS="-DCMAKE_TOOLCHAIN_FILE=../Client/cmake/Toolchains/android.toolchain.cmake"
COMMON_CMAKE_ARGS="$COMMON_CMAKE_ARGS -DANDROID_PLATFORM=android-${ANDROID_API}"
COMMON_CMAKE_ARGS="$COMMON_CMAKE_ARGS -DANDROID_ABI=arm64-v8a"
COMMON_CMAKE_ARGS="$COMMON_CMAKE_ARGS -DANDROID_STL=c++_static"
COMMON_CMAKE_ARGS="$COMMON_CMAKE_ARGS -DCONTRIB_PATH='${CONTRIB_PATH}'"
COMMON_CMAKE_ARGS="$COMMON_CMAKE_ARGS -DBOOST_ROOT='${CONTRIB_PATH}'/boost_1_55_0/"


# Simulate "armeabi-v7a with NEON".  We have to do this ourselves since we can't pass
# strings with spaces to -D<options> on the cmake command line.
COMMON_CMAKE_ARGS="$COMMON_CMAKE_ARGS -DNEON=1 -DVFPV3=1"

for ((i = 0; i < ${#BUILD_DIRECTORIES[@]}; i++)); do
    build_dir=${BUILD_DIRECTORIES[$i]}
    build_mode=${BUILD_MODES[$i]}

    if [[ ! -d $build_dir ]]; then
        echo "Creating build dir: $build_dir"
        mkdir -p "$build_dir"
    fi

    pushd "$build_dir" >/dev/null

    echo "== Configuring $build_mode in $(pwd) =="
    cmake $COMMON_CMAKE_ARGS \
          -DCMAKE_BUILD_TYPE=$build_mode \
          -G "$GENERATOR" \
          ../../Client "$@"

    echo
done