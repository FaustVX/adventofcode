original source: [https://adventofcode.com/2021/day/9](https://adventofcode.com/2021/day/9)
## --- Day 9: Smoke Basin ---
These caves seem to be [lava tubes](https://en.wikipedia.org/wiki/Lava_tube). Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

<pre>
<code>2<em>1</em>9994321<em>0</em>
3987894921
98<em>5</em>6789892
8767896789
989996<em>5</em>678
</code>
</pre>

Each number corresponds to the height of a particular location, where <code>9</code> is the highest and <code>0</code> is the lowest a location can be.

Your first goal is to find the <em>low points</em> - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

In the above example, there are <em>four</em> low points, all highlighted: two are in the first row (a <code>1</code> and a <code>0</code>), one is in the third row (a <code>5</code>), and one is in the bottom row (also a <code>5</code>). All other locations on the heightmap have some lower adjacent location, and so are not low points.

The <em>risk level</em> of a low point is <em>1 plus its height</em>. In the above example, the risk levels of the low points are <code>2</code>, <code>1</code>, <code>6</code>, and <code>6</code>. The sum of the risk levels of all low points in the heightmap is therefore <code><em>15</em></code>.

Find all of the low points on your heightmap. <em>What is the sum of the risk levels of all low points on your heightmap?</em>


