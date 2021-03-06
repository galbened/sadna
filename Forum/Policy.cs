﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum
{

    public class Policy
    {
        private int moderatorNum;
        private string passwordEnsuringDegree;
        private int minLength;
        private Boolean upperCase;
        private Boolean lowerCase;
        private Boolean numbers;
        private Boolean symbols;
        private Boolean canModeratorDeleteMessage;
        private int passwordExpectancy;  //in days
     

        public Policy()
        {
            moderatorNum = 2;
            passwordExpectancy = 20;
        }

        public Policy(int moderN, string passEnsDeg, Boolean upper, Boolean lower, Boolean nums, Boolean symbs, int mLen, int passExp)
        {
            moderatorNum = moderN;
            passwordEnsuringDegree = passEnsDeg;
            upperCase = upper;
            lowerCase = lower;
            numbers = nums;
            symbols = symbs;
            minLength = mLen;
            passwordExpectancy = passExp;
            canModeratorDeleteMessage = false;
        }

        public Boolean IsValid(string password)
        {
            Boolean up = !upperCase;
            Boolean low = !lowerCase;
            Boolean nums = !numbers;
            Boolean symbs = !symbols;
            if (password.Length < minLength)
                return false;
            for (int i = 0; i < password.Length; i++)
            {
                if (!up && password[i] >= 'A' && password[i] <= 'Z')
                {
                    up = true;
                    continue;
                }
                if (!low && password[i] >= 'a' && password[i] <= 'z')
                {
                    low = true;
                    continue;
                }
                if (!nums && password[i] >= '0' && password[i] <= '9')
                {
                    nums = true;
                    continue;
                }
                if (!symbs && ((password[i] > 32 && password[i] < 48)
                    || (password[i] > 57 && password[i] < 65) ||
                    (password[i] > 90 && password[i] < 97) ||
                    (password[i] > 122 && password[i] < 127)))
                {
                    symbs = true;
                    continue;
                }
            }
            return (up & low & nums & symbs);
        }

        internal Boolean CanModeratorDeleteMessage
        {
            get { return canModeratorDeleteMessage; }
            set { canModeratorDeleteMessage = value; }
        }

        internal int ModeratorNum
        {
            set { moderatorNum = value; }
            get { return moderatorNum; }
        }

        internal int PasswordExpectancy
        {
            get { return passwordExpectancy; }
            set { passwordExpectancy = value; }
        }

        internal string PasswordEnsuringDegree
        {
            set { passwordEnsuringDegree = value; }
        }

        internal Boolean UpperCase
        {
            set { upperCase = value; }
        }

        internal Boolean LowerCase
        {
            set { lowerCase = value; }
        }
        internal Boolean Numbers
        {
            set { numbers = value; }
        }
        internal Boolean Symbols
        {
            set { symbols = value; }
        }
        internal int MinLength
        {
            set { minLength = value; }
        }
    }
}
