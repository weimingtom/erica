SRCS     = $(wildcard *.cpp)
OBJS     = $(SRCS:.cpp=.o)
CXX      = g++
CXXFLAGS = -Wall -g -I../../src
LD       = g++
LDFLAGS  = -L../../src  
LIBS     = -lerica -lGL -lglut
TARGET   = a.out


all:  $(TARGET)

$(TARGET): $(OBJS)
	$(LD) $(LDFLAGS) $^ $(LIBS) -o $(TARGET)
	@echo done.

%.o: %.cpp
	$(CXX) $(CXXFLAGS) $< -c


c: clean

clean:
	rm -f *.o *.core *.png *.tiff *.hdr *~ *# $(TARGET)
	@echo done.
