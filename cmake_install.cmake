# Install script for directory: /mnt/f/Trunk2012/Client

# Set the install prefix
IF(NOT DEFINED CMAKE_INSTALL_PREFIX)
  SET(CMAKE_INSTALL_PREFIX "/home/watrabi/Android/ndk/android-ndk-r10e/toolchains/arm-linux-androideabi-4.9/prebuilt/linux-x86_64/user")
ENDIF(NOT DEFINED CMAKE_INSTALL_PREFIX)
STRING(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
IF(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  IF(BUILD_TYPE)
    STRING(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  ELSE(BUILD_TYPE)
    SET(CMAKE_INSTALL_CONFIG_NAME "Noopt")
  ENDIF(BUILD_TYPE)
  MESSAGE(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
ENDIF(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)

# Set the component getting installed.
IF(NOT CMAKE_INSTALL_COMPONENT)
  IF(COMPONENT)
    MESSAGE(STATUS "Install component: \"${COMPONENT}\"")
    SET(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  ELSE(COMPONENT)
    SET(CMAKE_INSTALL_COMPONENT)
  ENDIF(COMPONENT)
ENDIF(NOT CMAKE_INSTALL_COMPONENT)

# Install shared libraries without execute permission?
IF(NOT DEFINED CMAKE_INSTALL_SO_NO_EXE)
  SET(CMAKE_INSTALL_SO_NO_EXE "1")
ENDIF(NOT DEFINED CMAKE_INSTALL_SO_NO_EXE)

IF(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  INCLUDE("/mnt/f/Trunk2012/Client/boostlibs/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/ClientShared/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/Base/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/Log/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/Rendering/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/Network/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/App/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/Android/cmake_install.cmake")
  INCLUDE("/mnt/f/Trunk2012/Client/App.BulletPhysics/cmake_install.cmake")

ENDIF(NOT CMAKE_INSTALL_LOCAL_ONLY)

IF(CMAKE_INSTALL_COMPONENT)
  SET(CMAKE_INSTALL_MANIFEST "install_manifest_${CMAKE_INSTALL_COMPONENT}.txt")
ELSE(CMAKE_INSTALL_COMPONENT)
  SET(CMAKE_INSTALL_MANIFEST "install_manifest.txt")
ENDIF(CMAKE_INSTALL_COMPONENT)

FILE(WRITE "/mnt/f/Trunk2012/Client/${CMAKE_INSTALL_MANIFEST}" "")
FOREACH(file ${CMAKE_INSTALL_MANIFEST_FILES})
  FILE(APPEND "/mnt/f/Trunk2012/Client/${CMAKE_INSTALL_MANIFEST}" "${file}\n")
ENDFOREACH(file)
