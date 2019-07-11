using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Mode
{
    protected Main m_oMainRef = null;

    public Mode(Main o)
    {
        m_oMainRef = o;
    }

    public abstract void Start();
    public abstract void Stop();
}
