#include <vector>

extern "C" int add(int left, int right)
{
    std::vector<int> values;
    values.push_back(left);
    values.push_back(right);

    return values[0] + values[1];
}
