namespace IntegrationTests.Module
{
    public class Class1
    {
		private void TempFunc()
        {
            int num1 = 10000;
            int index2 = 1;
            bucket[] buckets = new bucket[2];
            buckets[0] = new bucket() {key = "a", val = "b", hash_coll = 5674};
            buckets[1] = new bucket() {key = "c", val = "d", hash_coll = 0};
            
            buckets[ index2 ].hash_coll |= ( int ) num1;
			
			var hashColl =  buckets[ index2 ].hash_coll;
			buckets[ index2 ].hash_coll = hashColl | (int)num1;
        }
        
        private struct bucket {
            public object key;
            public object val;
            public int hash_coll;
        }
    }
}