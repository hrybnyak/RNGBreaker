namespace RNGBreaker
{
    public static class BetterMTBreaker
    {
        public const uint a = 0x9908B0DF;
        public const uint d = 0xFFFFFFFF;
        public const uint c = 0xEFC60000;
        public const uint b = 0x9D2C5680;
        public const int u = 11;
        public const int s = 7;
        public const int t = 15;
        public const int l = 18;

        public static MT19937 BreakBetterMt(uint[] states)
        {
            var recoveredStates = new uint[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                var y1 = states[i] ^ (states[i] >> l);
                var y2 = y1 ^ ((y1 << t) & c);
                uint smask = (1 << s) - 1;
                var y3 = y2 ^ ((y2 << s) & b & (smask << s));
                var y4 = y3 ^ ((y3 << s) & b & (smask << (s * 2)));
                var y5 = y4 ^ ((y4 << s) & b & (smask << (s * 3)));
                var y6 = y5 ^ ((y5 << s) & b & (smask << (s * 4)));
                uint umask = (1 << u) - 1;
                var y7 = y6 ^ ((y6 >> u) & (umask << (u * 2)));
                var y8 = y7 ^ ((y7 >> u) & (umask << u));
                recoveredStates[i] = y8 ^ ((y8 >> u) & umask);
            }
            var mt = new MT19937();
            mt.SetInitialState(recoveredStates);
            return mt;
        } 
    }
}
