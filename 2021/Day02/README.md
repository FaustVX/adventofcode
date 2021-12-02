original source: [https://adventofcode.com/2021/day/2](https://adventofcode.com/2021/day/2)
## --- Day 2: Dive! ---
Now, you need to figure out how to pilot this thing.

It seems like the submarine can take a series of commands like <code>forward 1</code>, <code>down 2</code>, or <code>up 3</code>:


 - <code>forward X</code> increases the horizontal position by <code>X</code> units.
 - <code>down X</code> <em>increases</em> the depth by <code>X</code> units.
 - <code>up X</code> <em>decreases</em> the depth by <code>X</code> units.

Note that since you're on a submarine, <code>down</code> and <code>up</code> affect your <em>depth</em>, and so they have the opposite result of what you might expect.

The submarine seems to already have a planned course (your puzzle input). You should probably figure out where it's going. For example:

<pre>
<code>forward 5
down 5
forward 8
up 3
down 8
forward 2
</code>
</pre>

Your horizontal position and depth both start at <code>0</code>. The steps above would then modify them as follows:


 - <code>forward 5</code> adds <code>5</code> to your horizontal position, a total of <code>5</code>.
 - <code>down 5</code> adds <code>5</code> to your depth, resulting in a value of <code>5</code>.
 - <code>forward 8</code> adds <code>8</code> to your horizontal position, a total of <code>13</code>.
 - <code>up 3</code> decreases your depth by <code>3</code>, resulting in a value of <code>2</code>.
 - <code>down 8</code> adds <code>8</code> to your depth, resulting in a value of <code>10</code>.
 - <code>forward 2</code> adds <code>2</code> to your horizontal position, a total of <code>15</code>.

After following these instructions, you would have a horizontal position of <code>15</code> and a depth of <code>10</code>. (Multiplying these together produces <code><em>150</em></code>.)

Calculate the horizontal position and depth you would have after following the planned course. <em>What do you get if you multiply your final horizontal position by your final depth?</em>


