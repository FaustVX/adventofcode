original source: [https://adventofcode.com/2021/day/3](https://adventofcode.com/2021/day/3)
## --- Day 3: Binary Diagnostic ---
The submarine has been making some odd creaking noises, so you ask it to produce a diagnostic report just in case.

The diagnostic report (your puzzle input) consists of a list of binary numbers which, when decoded properly, can tell you many useful things about the conditions of the submarine. The first parameter to check is the <em>power consumption</em>.

You need to use the binary numbers in the diagnostic report to generate two new binary numbers (called the <em>gamma rate</em> and the <em>epsilon rate</em>). The power consumption can then be found by multiplying the gamma rate by the epsilon rate.

Each bit in the gamma rate can be determined by finding the <em>most common bit in the corresponding position</em> of all numbers in the diagnostic report. For example, given the following diagnostic report:

<pre>
<code>00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010
</code>
</pre>

Considering only the first bit of each number, there are five <code>0</code> bits and seven <code>1</code> bits. Since the most common bit is <code>1</code>, the first bit of the gamma rate is <code>1</code>.

The most common second bit of the numbers in the diagnostic report is <code>0</code>, so the second bit of the gamma rate is <code>0</code>.

The most common value of the third, fourth, and fifth bits are <code>1</code>, <code>1</code>, and <code>0</code>, respectively, and so the final three bits of the gamma rate are <code>110</code>.

So, the gamma rate is the binary number <code>10110</code>, or <code><em>22</em></code> in decimal.

The epsilon rate is calculated in a similar way; rather than use the most common bit, the least common bit from each position is used. So, the epsilon rate is <code>01001</code>, or <code><em>9</em></code> in decimal. Multiplying the gamma rate (<code>22</code>) by the epsilon rate (<code>9</code>) produces the power consumption, <code><em>198</em></code>.

Use the binary numbers in your diagnostic report to calculate the gamma rate and epsilon rate, then multiply them together. <em>What is the power consumption of the submarine?</em> (Be sure to represent your answer in decimal, not binary.)


