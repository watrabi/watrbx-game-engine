#pragma once
#include <sys/time.h>

struct timeb {
    time_t time;
    unsigned short millitm;
    short timezone;
    short dstflag;
};

inline void ftime(struct timeb* tb) {
    struct timeval tv;
    gettimeofday(&tv, nullptr);
    tb->time = tv.tv_sec;
    tb->millitm = tv.tv_usec / 1000;
    tb->timezone = 0;
    tb->dstflag = 0;
}
