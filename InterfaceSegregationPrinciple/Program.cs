using System;

namespace InterfaceSegregationPrinciple
{
    public class Document
    {

    }

    public interface IMachine
    {
        void Print(Document document);
        void Scan(Document document);
        void Fax(Document document);
    }

    public interface IPrinter
    {
        void Print(Document document);
    }

    public interface IScanner
    {
        void Scan(Document document);
    }

    public interface IFax
    {
        void Fax(Document document);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document document)
        {
            //
        }

        public void Print(Document document)
        {
            //
        }

        public void Scan(Document document)
        {
            //
        }
    }

    // Poor implementation
    public class OldFashionedPrinter : IMachine
    {
        public void Fax(Document document)
        {
            //
        }

        public void Print(Document document)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document document)
        {
            throw new NotImplementedException();
        }
    }

    public class OldFashionedPrinterImproved : IPrinter
    {
        public void Print(Document document)
        {
            //
        }
    }

    public interface IMultiFunctionDevice: IScanner,IPrinter //..
    {

    }

    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        private IPrinter _printer;
        private IScanner _scanener;

        public MultiFunctionMachine(IPrinter printer,IScanner scanner)
        {
            this._printer = printer ?? throw new ArgumentNullException(nameof(printer));
            this._scanener = scanner ?? throw new ArgumentNullException(nameof(scanner));
        }

        public void Print(Document document)
        {
            _printer.Print(document);
        }

        public void Scan(Document document)
        {
            _scanener.Scan(document);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
